+++
date = 2019-11-07T10:54:26Z
description = ""
draft = false
slug = "baremetal-kubernetes-cluster-networking-and-load-balancer"
title = "Baremetal Kubernetes Cluster - Networking and Load Balancer"

+++


This is [part of a series](__GHOST_URL__/baremetal-kubernetes-cluster-start-to-finish/) on creating a Kubernetes cluster. In the [previous post](__GHOST_URL__/baremetal-kubernetes-cluster-persistant-volumes/), we provisioned the persistent storage for our applications. In this post, we'll start to build out the networking stack for our cluster.

Before we get deep into the various networking objects necessary for a fully-functioning cluster, I want to give a high-level overview of [networking in k8s](https://kubernetes.io/docs/concepts/cluster-administration/networking/). I recommend that you first get familiar with all the pieces involved, then do some more in-depth reading on each aspect to better understand what's happening behind the scenes. Networking in k8s is a complex topic with many facets. The information provided here should be enough to get you started and provide what we need for our application.

In k8s, networking is virtualized inside the cluster. There are a number of k8s network implementations, some specific to cloud providers. We used `flannel` when [creating out cluster](__GHOST_URL__/baremetal-kubernetes-cluster-create-the-cluster/).

[`Pods`](https://kubernetes.io/docs/concepts/workloads/pods/pod/) are assigned private IP addresses inside the cluster. These addresses can change as Pods are created and destroyed. In order to reach the applications running in those Pods, requests are made to the cluster via a [`Service`](https://kubernetes.io/docs/concepts/services-networking/service/). [Services](https://kubernetes.io/docs/concepts/services-networking/service/) provide a single IP that will not change for the life of the Service and are backed by one or more Pods. The Pods backing the Service might change or scale, but the Service will be consistently available for requests.

A Service is likely only valuable if it is accessible from outside the cluster on an external IP. Kubernetes supports this in two ways, via `NodePort` and `LoadBalancer`. [NodePort](https://kubernetes.io/docs/concepts/services-networking/service/#nodeport) is simply when a Service is exposed on a port on the node's IP. You can access the Service from inside the cluster by default, but you have to `kubectl expose` the Service if you want to access it from outside. A [LoadBalancer](https://kubernetes.io/docs/concepts/services-networking/service/#loadbalancer) can also be used to expose Services, and is typically provided by a cloud platform, such as [AWS](https://docs.aws.amazon.com/elasticloadbalancing/latest/network/introduction.html) or [GCE](https://cloud.google.com/load-balancing/). The LoadBalancer would make the Service publicly available by assigning an IP from a block of public addresses and then route requests to the Service.

The official docs have [a great article](https://kubernetes.io/docs/concepts/services-networking/connect-applications-service/) that describes these concepts in more detail. I also recommend [this very helpful blog post](https://blog.getambassador.io/kubernetes-ingress-nodeport-load-balancers-and-ingress-controllers-6e29f1c44f2d) about various Ingress strategies for k8s.

Because we're creating a baremetal k8s cluster, we will not have access to a cloud load balancer. However, there is a project called [MetalLB](https://metallb.universe.tf/) that can provide the same load balancing in our cluster.

First [install MetalLB](https://metallb.universe.tf/installation/) using their `yaml` file.

`kubectl apply -f https://raw.githubusercontent.com/google/metallb/v0.8.3/manifests/metallb.yaml`

This will deploy the components of MetalLB, but they will be in a pending state, waiting for a `ConfigMap` to [be deployed](https://metallb.universe.tf/configuration/), defining how the load balancer will issue IP addresses.

Save the following `yaml` into a file named `metallb-config.yaml`. Substitute an appropriate address range based on your network and which IP addresses could be reserved for the load balancer. In my case, my DHCP server does not issue addresses in this range, and my DHCP reservations also do not use these addresses.

```
apiVersion: v1
kind: ConfigMap
metadata:
  namespace: metallb-system
  name: config
data:
  config: |
    address-pools:
    - name: default
      protocol: layer2
      addresses:
      - 192.168.1.160-192.168.1.169
```

Now that we understand a little about networking in Kubernetes and created a LoadBalancer, we are ready to dig a little deeper and [create an Ingress Controller](__GHOST_URL__/baremetal-kubernetes-cluster-ingress-controller/).

