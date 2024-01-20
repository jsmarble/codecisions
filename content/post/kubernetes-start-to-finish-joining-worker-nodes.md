---
date: "2019-11-07T00:43:00Z"
description: ""
draft: false
slug: kubernetes-start-to-finish-joining-worker-nodes
title: Baremetal Kubernetes Cluster - Joining Worker Nodes
---


This is [part of a series](__GHOST_URL__/baremetal-kubernetes-cluster-start-to-finish/) on creating a Kubernetes cluster. In the [previous post](__GHOST_URL__/baremetal-kubernetes-cluster-creating-the-cluster/), we created the Kubernetes cluster and master node. In this post, we'll be joining the worker nodes to the Kubernetes (k8s) cluster.

Worker nodes are intended to be configured by the master node, therefore there isn't any configuration necessary on the node itself. As long as the same version of Kubernetes and Docker are installed on the server, it should work in the cluster. It is worth noting that all dependencies for the system should also be installed on each node. For example, we'll be using NFS storage for pods, so the `nfs-common` package needs to be installed anywhere that an NFS share will be mounted. If you used my scripts when [installing Kubernetes](__GHOST_URL__/baremetal-kubernetes-cluster-installing-kubernetes/), this is already done.

Since we already installed Kubernetes on each node in [a previous post](__GHOST_URL__/baremetal-kubernetes-cluster-installing-kubernetes/), we're ready to join the nodes to the cluster. We need the join command from the [previous post](__GHOST_URL__/baremetal-kubernetes-cluster-create-the-cluster/) when we installed k8s. Simply run it using `sudo`, and it will join the cluster.

`kubeadm join 192.168.1.150:6443 --token tf472d.yuy8rqkjqcgxs29w --discovery-token-ca-cert-hash sha256:0d969538bfef1ae75ba5c327e7148a18c49f9e762632141edf4d432ada895942`

After it finishes, you can run `kubectl get nodes` on the master node and see the new node in the cluster.

Optionally, you can label the node so it shows its role as `Node`. Run this command, replacing `<node_name>` with the name of the node (i.e. `k8s1`).

`kubectl label node <node_name> node-role.kubernetes.io/node=`

Repeat these steps for each node in the cluster.

Now that we have a working cluster, we need to start configuring it. To make configuration easier in the future, [we will first install Helm](__GHOST_URL__/baremetal-kubernetes-cluster-helm/).

