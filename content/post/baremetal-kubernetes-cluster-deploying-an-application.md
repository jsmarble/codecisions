+++
date = 2019-11-07T11:18:17Z
description = ""
draft = false
slug = "baremetal-kubernetes-cluster-deploying-an-application"
title = "Baremetal Kubernetes Cluster - Deploying An Application"

+++


This is [part of a series](__GHOST_URL__/baremetal-kubernetes-cluster-start-to-finish/) on creating a Kubernetes cluster. In the [previous post](__GHOST_URL__/baremetal-kubernetes-cluster-ingress-controller/), we dove deeper into Kubernetes networking and created an Ingress Controller to make our Services accessible. In this post, we'll be deploying an application to the Kubernetes (k8s) cluster.

If you are setting up your own baremetal k8s cluster, I assume there's a good chance that you also already run a [Plex Media Server](https://www.plex.tv/). Therefore, I thought it would fun to deploy [Tautulli](https://tautulli.com/), a monitoring application for Plex, as the first application in the cluster. Specifically, we'll be deploying the [docker image for Tautilli](https://hub.docker.com/r/linuxserver/tautulli) that is maintained by the excellent people at [LinuxServer.io](https://linuxserver.io).

First, define a `Deployment` for the application. [Deployments](https://kubernetes.io/docs/concepts/workloads/controllers/deployment/) tell k8s how an application should be managed, such as what image to use, how many replicas should run, what volumes should be mounted, etc. A similar concept, called [ReplicaSets](https://kubernetes.io/docs/concepts/workloads/controllers/replicaset/), is sometimes used in guides on the internet. I recommend that you follow [the guidance of the Kubernetes team](https://kubernetes.io/docs/concepts/workloads/controllers/replicaset/#when-to-use-a-replicaset) and just stick with Deployments.

Save the following `yaml` into a file named `tautulli-deployment.yaml`:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: tautulli
  labels:
    app: tautulli
spec:
  replicas: 1
  selector:
    matchLabels:
      app: tautulli
  template:
    metadata:
      labels:
        app: tautulli
    spec:
      containers:
        - name: tautulli
          image: linuxserver/tautulli
          env:
            - name: TZ
              value: "America/Chicago"
          resources:
            limits:
              memory: "2Gi"
            requests:
              memory: "1Gi"
          ports:
            - containerPort: 8181
              name: tautulli-web
          volumeMounts:
            - mountPath: /config
              name: default-pvc
              subPath: tautulli
      volumes:
        - name: default-pvc
          persistentVolumeClaim:
            claimName: nfs
```

I'll explain what this defines, starting at the top. First, this is a `Deployment` named `tautulli`. It has a label called `app` with a value of `tautulli`. This label will help match up the Service to the Deployment later.

The `spec` indicates it should only have 1 `replica`. It also has `matchLabels` defined to match a `label` named `app` with a value of `tautulli`. This needs to match the `template`  `label`. I know this seems redundant, but it's required [per the official docs](https://kubernetes.io/docs/concepts/workloads/controllers/deployment/#selector).

The `template`  `spec` indicates it will run a `container` with the docker `image` of `linuxserver/tautulli`. It will be created with an environment variable (`env`) named `TZ` with a value of `America/Chicago`. You may update this with your appropriate time zone, which can be found using [this online tool](http://www.timezoneconverter.com/cgi-bin/findzone/findzone.tzc).

The `container` will also request `resources` of at least 1 GB of `memory`, limited to 2 GB. The container will also expose port 8181.

The `container` will mount a `volume` named `default-pvc` at a subPath of `tautulli` to the local path `/config`. This means your NFS share will have a new directory created called `tautulli` that will contain the config for the application. This allows the Pod to be recreated without losing your config.

The `template`, therefore, needs to define a `volume` named `default-pvc`. It will provide this volume by binding to the `persistentVolumeClaim` named `nfs` (see earlier post).

We can apply this to the cluster using `kubectl apply -f tautulli-deployment.yaml`.

Now that the application is deployed, we need to define a Service that will expose the Pods outside the cluster network.

Save the following `yaml` into a file named `tautulli-service.yaml`:

```yaml
kind: Service
apiVersion: v1
metadata:
  name: tautulli
spec:
  selector:
    app: tautulli
  ports:
    - protocol: TCP
      port: 8181
      targetPort: 8181
      name: tautulli-web
```

This is much easier to understand. The `Service` named `tautulli` will expose TCP on `port` 8181 to `targetPort` 8181 on Pods matching the label `app` with value `tautulli`. This exposed port will be named `tautulli-web` for later reference.

Apply this to the cluster using `kubectl apply -f tautulli-service.yaml`.

The only piece missing now is the Ingress that will allow our host request to the cluster to be router to the appropriate Service.

Save the following `yaml` into a file named `tautulli-ingress.yaml`:

```
apiVersion: extensions/v1beta1
kind: Ingress
metadata:
  name: tautulli-web
spec:
  rules:
  - host: tautulli.mydomain.com
    http:
      paths:
      - path: /
        backend:
          serviceName: tautulli
          servicePort: tautulli-web
```

This is perhaps the easiest to understand. An `Ingress` named `tautulli-web` contains a `rule` that requests for `host`  `tautulli.mydomain.com` should be router to the Service named `tautulli` on the `servicePort` named `tautulli-web`.

Apply this to the cluster using `kubectl apply -f tautulli-ingress.yaml`.

This will cause the Ingress Controller `nginx-ingress` to create a new site rule that is a reverse proxy to the Service defined in the Ingress.

Assuming you have the domain `tautulli.mydomain.com` pointed to your public IP address and the appropriate firewall port-forwarding rule to forward port 80/443 to your k8s master node IP address, your service should be accessible from the internet.

If you do not have the domain name and port forwarding setup yet, you can still test out the Ingress with `curl`. [Curl](https://curl.haxx.se/docs/manpage.html) allows you to specify headers for an HTTP request and view the response. We can use this to tell the Ingress Controller that we're requesting `tautulli.mydomain.com` and make that request directly to the external IP of the Ingress Controller. You can find the external IP using the command at the bottom of [the Ingress Controller post](__GHOST_URL__/baremetal-kubernetes-cluster-ingress-controller/).

Here's a `curl` example specifying the host header.

`curl -H "Host: tautulli.mydomain.com" "http://192.168.1.160/home"`

If everything is working, the response should include a lot of HTML from Tautulli.

Assuming your domain name is working correctly and port 80 is forwarded to your k8s load balancer IP for the ingress controller service (192.168.1.160 in my example), you should be able to navigate to [http://tautulli.mydomain.com](http://tautulli.mydomain.com) and be prompted to setup Tautulli with the connection to your Plex server.

So, there you have it. All of our hard work has finally paid off, and we are hosting Tautulli in our Kubernetes cluster, taking advantage of all the benefits that come with it. It's time to [wrap up this series with a few parting thoughts](__GHOST_URL__/baremetal-kubernetes-cluster-final-thoughts/).

