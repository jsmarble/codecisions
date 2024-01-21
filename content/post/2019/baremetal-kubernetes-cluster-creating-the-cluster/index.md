---
date: "2019-11-06T20:46:38Z"
description: ""
draft: false
slug: baremetal-kubernetes-cluster-creating-the-cluster
title: Baremetal Kubernetes Cluster - Creating the Cluster
---


This is [part of a series](__GHOST_URL__/baremetal-kubernetes-cluster-start-to-finish/) on creating a Kubernetes cluster. In the [previous post](__GHOST_URL__/baremetal-kubernetes-cluster-installing-kubernetes/), we installed Kubernetes on the servers. In this post, we'll be creating the Kubernetes (k8s) cluster and configuring the master node.

To get started, we first need to [create the cluster](https://kubernetes.io/docs/setup/production-environment/tools/kubeadm/create-cluster-kubeadm/). There are two parts to creating the cluster.

1. Initialize the cluster
2. Create the [pod network](https://kubernetes.io/docs/setup/production-environment/tools/kubeadm/create-cluster-kubeadm/#pod-network)

Login to the master node and execute the following command to create the cluster using this server as the master node and an internal pod network of `10.244.0.0/16` (the default in the official docs).

`sudo kubeadm init --pod-network-cidr=10.244.0.0/16`.

After the command completes, you need to look at the end of the output and copy the `kubeadm join` command and save it for later use when joining the worker nodes to the cluster.

{{< figure src="__GHOST_URL__/content/images/2019/11/kubeadm-join-2.png" caption="Copy the <code>kubeadm join</code> command" >}}

Also, if you want to be able to run k8s commands using this user, you need to run the commands in the output to configure the user with access to the cluster.

{{< figure src="__GHOST_URL__/content/images/2019/11/k8s-user-1.png" caption="Run these commands to access k8s as your user" >}}

Before your cluster can become operational, it needs to have a pod network configured. This is what the cluster will use to communicate with the pods and manage the cluster's internal network.

The most common homelab pod network is called [`flannel`](https://coreos.com/flannel/docs/latest/kubernetes.html). It can easily be added to your cluster by applying its configuration yaml file to the cluster. To apply flannel to your cluster, run this [apply command](https://kubernetes.io/docs/reference/generated/kubectl/kubectl-commands). _Note: If you changed the CIDR above, you will need to apply a modified copy of the file linked below with the correct CIDR._

```bash
kubectl apply -f https://raw.githubusercontent.com/coreos/flannel/2140ac876ef134e0ed5af15c65e414cf26827915/Documentation/kube-flannel.yml
```

You can see the pods that are deploying by running `kubectl get pods -n kube-system`.

Once the pods are deployed, your master node should show as ready when you run `kubectl get nodes`.

Now that the cluster is created and the master is operational, it's time to [join the worker nodes](__GHOST_URL__/kubernetes-start-to-finish-joining-worker-nodes/).

