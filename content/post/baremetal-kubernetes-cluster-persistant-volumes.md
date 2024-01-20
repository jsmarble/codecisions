+++
date = 2019-11-07T10:13:13Z
description = ""
draft = false
slug = "baremetal-kubernetes-cluster-persistant-volumes"
title = "Baremetal Kubernetes Cluster - Persistent Volumes"

+++


This is [part of a series](__GHOST_URL__/baremetal-kubernetes-cluster-start-to-finish/) on creating a Kubernetes cluster. In the [previous post](__GHOST_URL__/baremetal-kubernetes-cluster-helm/), we installed Helm to make deployments easier. In this post, we'll start configuring our new Kubernetes (k8s) cluster by setting up persistent storage.

In any containerized system, there is likely the need for data to persistent after the container is gone. Commonly, that data is some configuration for the application inside the container, but it may also be files that are used by the container. For example, in a container for a photo management app, when a user uploads a photo to the app, it gets stored on a volume somewhere. If that container is stopped or deleted, the users would appreciate all their photos not being lost.

Kubernetes further increases the need for persistent storage because of the ephemeral nature of the cluster. A node may be taken out of service, resource constraints may require containers to be shifted around, or an application might be scaled to multiple instances.

Kubernetes uses [volumes](https://kubernetes.io/docs/concepts/storage/volumes/) to provide a container with persistent storage. A volume is mounted in a container at a specified path, then the container uses that path as if it were the local file system. In k8s, this is accomplished via a `Persistent Volume` and `Persistent Volume Claim`.

A `[Persistent Volume](https://kubernetes.io/docs/concepts/storage/persistent-volumes/)` (`PV`) is a volume that has either been provisioned by a cluster administrator for future use or has been dynamically provisioned by a `Storage Class`. Various cloud providers and other plugins provide the ability to dynamically provision storage upon request. For our cluster, we're going to be using `NFS` storage, so we will be statically provisioning it.

A `[Persistent Volume Claim](https://kubernetes.io/docs/concepts/storage/persistent-volumes/)` (`PVC`) is a request for storage provided by a `Persistent Volume`. There can only be one `PVC` bound to a `PV` at any given time. Once the `PVC` releases its claim, the `PV` can be [released or deleted](https://kubernetes.io/docs/concepts/storage/persistent-volumes/#lifecycle-of-a-volume-and-claim). The `PVC` will be used by the container to mount a path to the volume.

For our purposes, we will setup a `PV` that is attached to an `NFS` share on a storage server. Creating an `NFS` share is beyond the scope of this article, but you will need one available before continuing.

You configure objects in k8s by using the `kubectl` command to apply a `yaml` file. Sometimes these `yaml` files are on your server, and sometimes they are on the internet. You can apply them either way. In fact, you already applied one when you [configured the cluster](__GHOST_URL__/baremetal-kubernetes-cluster-create-the-cluster/) with the `flannel` network. We'll be writing our own `yaml` file on the master node to create our `PV`.

Login to the master node using `ssh`.

Save the following `yaml` into a file named `nfs-pv.yaml`, replacing `<nfs-server>` and `<nfs-path>` with your appropriate share details. This will create a `PV` with 1 GB of capacity.

```yaml
apiVersion: v1                             
kind: PersistentVolume                     
metadata:                                  
  name: nfs                                
spec:                                      
  capacity:                                
    storage: 1Gi                           
  volumeMode: Filesystem                   
  storageClassName: slow                   
  persistentVolumeReclaimPolicy: Retain    
  accessModes:                             
    - ReadWriteMany                        
  nfs:                                     
    server: <nfs-server>
    path: "<nfs-path>" 
```

Apply the `yaml` file to the cluster: `kubectl apply -f nfs-pv.yaml`

The `PV` is now ready to be bound to a `PVC`.

Save the following `yaml` into a file named `nfs-pvc.yaml`. Note the `volumeName` parameter matches the `name` parameter of the above `PV`. If you create additional `PV` and `PVC` objects later, be sure to use the corresponding name.

```
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: nfs
spec:
  accessModes:
    - ReadWriteMany
  storageClassName: slow
  resources:
    requests:
      storage: 1Gi
  volumeName: "nfs"
```

Apply the `yaml` file to the cluster: `kubectl apply -f nfs-pvc.yaml`

Now that we have persistent storage provisioned and available for use, we need to [configure our networking](__GHOST_URL__/baremetal-kubernetes-cluster-networking-and-load-balancer/).

