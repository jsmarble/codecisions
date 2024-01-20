---
date: "2019-11-07T11:05:07Z"
description: ""
draft: false
slug: baremetal-kubernetes-cluster-ingress-controller
title: Baremetal Kubernetes Cluster - Ingress Controller
---


This is [part of a series](__GHOST_URL__/baremetal-kubernetes-cluster-start-to-finish/) on creating a Kubernetes cluster. In the [previous post](__GHOST_URL__/baremetal-kubernetes-cluster-networking-and-load-balancer/), we learned some Kubernetes networking concepts and added a Load Balancer to the cluster. In this post, we'll dive deeper into networking by creating an Ingress Controller.

Before we start creating things, let's take a moment to think about our networking situation and limitations in a homelab.

Since our cluster will be running in a homelab, we likely only have a single public IP address available. Consider what happens if you have a web service running in your cluster that you want to access from the Internet. You have a domain name set up that points to your public IP, and you want to be able to access your web service at that domain on the standard `HTTP(S)` ports. You could configure your firewall to port forward 80/443 from your public IP to the external IP address for your Service (allocated by the Load Balancer from the previous post), but what about the next Service? You can create another domain name, but you still only have one option for port-forwarding. How do you pass all `HTTP(S)` traffic to your cluster, but have it routed to the correct Service?

Kubernetes provides a solution through an [`Ingress`](https://kubernetes.io/docs/concepts/services-networking/ingress/). [Ingresses](https://kubernetes.io/docs/concepts/services-networking/ingress/) solve this by providing load balancing and domain-based request routing to Services. An Ingress only operates on `HTTP(S)` traffic, so any other ports or protocols should use NodePort or LoadBalancer. An Ingress is configured with the host and the corresponding Service to which requests will be routed.

In order for an Ingress to be functional, we need to create an `Ingress Controller`. An [Ingress Controller](https://kubernetes.io/docs/concepts/services-networking/ingress-controllers/) is the engine behind the scenes that is routing requests to Services based on defined Ingresses. Most cloud platforms provide their own Ingress Controller in their native services. You can also use `[nginx](https://www.nginx.com/)` or `[traefik](https://github.com/containous/traefik)` on a baremetal cluster. We'll be using Nginx.

An important thing to notice is that there are two Ingress Controllers out there that both use Nginx. One is maintained by the Kubernetes team, called `[ingress-nginx](https://github.com/kubernetes/ingress-nginx)`. The other is maintained by the Nginx team, called `[nginx-ingress](https://www.nginx.com/products/nginx/kubernetes-ingress-controller)`. I could not get `ingress-nginx` to work properly in my past experience, but I had no problem getting `nginx-ingress` running. We'll be using `nginx-ingress` because it's easy to setup and is supported by the official Nginx team.

To get `nginx-ingress` installed, we will use Helm, which [we installed in an earlier post](__GHOST_URL__/baremetal-kubernetes-cluster-helm/).

Our cluster is using the latest k8s release, v1.16, which had breaking changes to some APIs. That means we must use the newest `nginx-ingress`  [helm chart](https://hub.helm.sh/charts/nginx-edge/nginx-ingress), which is not yet published to the repo as of this writing. Fortunately, we can install the latest chart from their [git repo](https://github.com/nginxinc/kubernetes-ingress).

Our minimal server does not yet have git, so first we must install it.

`sudo apt install -y git`

Then, we can clone the `nginx-ingress` repo and install the chart. Pass the parameter `--set controller.service.loadBalancerIP=192.168.1.160` to request the LoadBalancer to assign a specific IP address. This will be useful because we need a known IP to port-foward from the router.

```
git clone https://github.com/nginxinc/kubernetes-ingress/
cd kubernetes-ingress/deployments/helm-chart
helm install --name nginx-ingress --namespace nginx-ingress --set controller.service.loadBalancerIP=192.168.1.160 .
```

Now, we can check what services and pods are running in our `nginx-ingress` namespace.

`kubectl get service -n nginx-ingress`

{{< figure src="__GHOST_URL__/content/images/2019/11/nginx-service.png" >}}

Notice that the LoadBalancer has assigned our requested EXTERNAL-IP to the Service.

When calling `kubectl get pods`, we can include the `-o wide` flag to see which node Pods are running on.

`kubectl get pods -n nginx-ingress -o wide`

{{< figure src="__GHOST_URL__/content/images/2019/11/nginx-pods-2.png" >}}

We now have an Ingress Controller to make our applications accessible from outside the cluster. When we deploy Services, we simply have to deploy an Ingress alongside it and provide which host will route to the Service.

Now, it's time to [deploy an application](__GHOST_URL__/baremetal-kubernetes-cluster-deploying-an-application/)!

