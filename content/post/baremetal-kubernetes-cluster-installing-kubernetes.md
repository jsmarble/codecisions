+++
date = 2019-11-06T07:54:42Z
description = ""
draft = false
slug = "baremetal-kubernetes-cluster-installing-kubernetes"
title = "Baremetal Kubernetes Cluster - Installing Kubernetes"

+++


This is [part of a series](__GHOST_URL__/baremetal-kubernetes-cluster-start-to-finish/) on creating a Kubernetes cluster. In the [previous post](__GHOST_URL__/baremetal-kubernetes-cluster-creating-the-virtual-machines/), we installed Debian 10 onto four virtual machines that will be used in the cluster. In this post, we will be installing Kubernetes (k8s) on them.

Now that the machines are ready, it's time to install k8s and docker. This will be the same process for the master node and worker nodes. The [official steps](https://kubernetes.io/docs/setup/production-environment/tools/kubeadm/install-kubeadm/) mainly involve setting up the necessary package sources then installing the packages and dependencies. There are also required system changes and recommended configurations for docker.

I have [created a script](https://gitlab.com/snippets/1909377) to make this easier, since we'll be repeating this several times. Note that my scripts install additional dependencies and combine [other suggested configurations](https://kubernetes.io/docs/setup/production-environment/container-runtimes/) from the docs. You are welcome to use my script or perform the steps manually. If you choose to do it manually, I suggest you read the [official docs](https://kubernetes.io/docs/setup/production-environment/tools/kubeadm/install-kubeadm/) and use my script as a reference.

To use my script to install k8s, use these commands.

```
wget -O k8s-debian.sh https://gitlab.com/snippets/1909377/raw
sudo sh k8s-debian.sh
```

Now that the server is fully setup with k8s, repeat the process for each of the worker nodes. All steps until now apply to both master and worker nodes.

Next we'll [create the cluster](__GHOST_URL__/baremetal-kubernetes-cluster-creating-the-cluster/).

