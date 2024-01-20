+++
date = 2019-11-07T11:45:10Z
description = ""
draft = false
slug = "baremetal-kubernetes-cluster-final-thoughts"
title = "Baremetal Kubernetes Cluster - Final Thoughts"

+++


This is the conclusion of [a series on creating a Kubernetes cluster](__GHOST_URL__/baremetal-kubernetes-cluster-start-to-finish/). In the [previous post](__GHOST_URL__/baremetal-kubernetes-cluster-deploying-an-application/), we deployed an application to the Kubernetes (k8s) cluster and saw everything working end-to-end.

I hope you enjoyed your journey with me and learned a lot along the way. It's been fun and challenging to create this series, and I have to say I am happy to have my cluster back up and running. I hope you find many things to host on your shiny new k8s cluster.

Now, all that remains is to highlight a few areas to explore from here.

We did not address `SSL/TLS` for accessing our Services. It is possible to configure a Service to use SSL certificates. The Kubernetes docs have [a section on securing your services](https://kubernetes.io/docs/concepts/services-networking/connect-applications-service/#securing-the-service) to get you started. Another option, which is what I do, is to put an nginx reverse proxy in front of your k8s cluster and proxy certain requests to your `nginx-ingress` external IP. This has a few benefits. It keeps your Services only concerned with their application. It allows you to forward other `HTTP(S)` requests to hosts outside of k8s. It allows you to setup a wildcard cert once for your domain, then define multiple Ingresses with subdomains that all get securely tunneled without additional configuration. Of course, the drawback to this approach is you have to configure and maintain nginx yourself, which isn't necessarily a bad thing. Maybe I'll write a future post on how I do this.

We did not discuss upgrading your deployed services. From my experience, this is as simple as deleting the current Pod and letting k8s deploy another instance. You can also scale your replicas down to zero, then back to your desired number. If you didn't specify a version in your Deployment, it will download the latest version when creating new Pods.

We did not cover `namespaces`. [Namespaces](https://kubernetes.io/docs/concepts/overview/working-with-objects/namespaces/) are just a way to separate objects in k8s. The convenient thing is that you can delete a namespace and everything in it is gone. This is nice when you are trying something that didn't work and just want it all cleaned up. The annoying thing about namespaces is that you cannot access a `PVC` from another namespace. This means if you already have an NFS mount to some data, you either have to jam whatever else needs it into the same namespace or create a whole new `PV` in order to bind a new `PVC` in the other namespace. Also, if running `kubectl` commands, you must specify the namespace in the `-n` paramter or else you will only see results from the `default` namespace. You can also specify `--all-namespaces` to see everything.

We did not look at the [Kubernetes Dashboard](https://kubernetes.io/docs/tasks/access-application-cluster/web-ui-dashboard/), which is a nice way to see an overview of your cluster. I don't find it particularly helpful for managing the cluster, though. Applying `yaml` files is easier. But it is nice to look at!

Best wishes on your future Kubernetes adventures!



