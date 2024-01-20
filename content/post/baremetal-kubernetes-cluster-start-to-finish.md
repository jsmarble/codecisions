---
date: "2019-11-06T02:12:35Z"
description: ""
draft: false
slug: baremetal-kubernetes-cluster-start-to-finish
title: Baremetal Kubernetes Cluster - Start to Finish
---


When I first setup my [Kubernetes](https://kubernetes.io/docs/concepts/overview/what-is-kubernetes/) (k8s) cluster in my [homelab](https://b3n.org/homelab-ideas/), it was a big undertaking and an arduous learning experience. It's been running solidly for months, but I recently tried upgrading a bit too bleeding edge, and it stopped working. After some unsuccessful attempts at recovery, I decided to rebuild the entire cluster.

Since I will be setting up a new cluster, I thought I would write this blog series to document each step so that anyone interested in learning k8s will be able to get a working environment. I will be covering [creating the virtual machines](__GHOST_URL__/baremetal-kubernetes-cluster-creating-the-virtual-machines/), [installing k8s](__GHOST_URL__/baremetal-kubernetes-cluster-installing-kubernetes/), [creating the cluster](__GHOST_URL__/baremetal-kubernetes-cluster-create-the-cluster/), [joining nodes to the cluster](__GHOST_URL__/kubernetes-start-to-finish-joining-worker-nodes/), [installing Helm](__GHOST_URL__/baremetal-kubernetes-cluster-helm/), [setting up persistent storage](__GHOST_URL__/baremetal-kubernetes-cluster-persistant-volumes/), [configuring networking](__GHOST_URL__/baremetal-kubernetes-cluster-networking-and-load-balancer/), [adding an Ingress Controller](__GHOST_URL__/baremetal-kubernetes-cluster-ingress-controller/), and [deploying applications](__GHOST_URL__/baremetal-kubernetes-cluster-deploying-an-application/).

Before you get started, you will want to familiarize yourself with some fundamental Kubernetes concepts. I will explain some topics in more detail as we go throughout the series, but it would be helpful to have a basic understanding to start. I recommend [this great "Kubernetes 101" article](https://medium.com/google-cloud/kubernetes-101-pods-nodes-containers-and-clusters-c1509e409e16) to start learning the terminology and architecture.

This series assumes working knowledge of server concepts, Linux commands, and a suitable VM hypervisor. I'll be using [VMWare ESXi](https://www.vmware.com/go/get-free-esxi), which is an enterprise hypervisor available for free [with some limitations](https://www.vladan.fr/esxi-5-free-whats-the-limitations/).

So, let's get started with [creating the virtual machines](__GHOST_URL__/baremetal-kubernetes-cluster-creating-the-virtual-machines/).

