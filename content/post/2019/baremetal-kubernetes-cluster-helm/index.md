---
date: "2019-11-07T02:40:00Z"
description: ""
draft: false
slug: baremetal-kubernetes-cluster-helm
title: Baremetal Kubernetes Cluster - Helm
---


This is [part of a series](__GHOST_URL__/baremetal-kubernetes-cluster-start-to-finish/) on creating a Kubernetes cluster. In the [previous post](__GHOST_URL__/kubernetes-start-to-finish-joining-worker-nodes/), we finalized the cluster by joining the worker nodes. In this post, we'll add one additional tool to our master node to help with Kubernetes (k8s) deployments.

Helm is a package manager for k8s, which makes it easy to deploy applications using a package, called a `[chart](https://helm.sh/docs/developing_charts/)`, that someone else has built. To install Helm, use the command below.

`curl -L https://git.io/get_helm.sh | bash`

Once Helm has installed, initialize it with `helm init`.

There are two parts to Helm: The Helm client (`helm`) and the Helm server (`tiller`). The server does the work to configure the cluster, so it needs to be granted permission to be a cluster admin.

First, create a service account for tiller.

`kubectl create serviceaccount tiller --namespace kube-system`

Then, create a `tiller-clusterrolebinding.yaml` file with these contents:

```yaml
kind: ClusterRoleBinding
apiVersion: rbac.authorization.k8s.io/v1beta1
metadata:
  name: tiller-clusterrolebinding
subjects:
- kind: ServiceAccount
  name: tiller
  namespace: kube-system
roleRef:
  kind: ClusterRole
  name: cluster-admin
  apiGroup: ""
```

Finally, deploy the `ClusterRoleBinding`, and init Helm again to use the service account.

```bash
kubectl create -f tiller-clusterrolebinding.yaml
helm init --service-account tiller --upgrade
```

Helm will be very useful in the future as we deploy additional components to our cluster. But first, we need to [give our cluster some persistent storage](__GHOST_URL__/baremetal-kubernetes-cluster-persistant-volumes/).

