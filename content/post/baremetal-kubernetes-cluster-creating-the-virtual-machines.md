+++
date = 2019-11-06T02:13:41Z
description = ""
draft = false
slug = "baremetal-kubernetes-cluster-creating-the-virtual-machines"
title = "Baremetal Kubernetes Cluster - Creating the Virtual Machines"

+++


This is [part of a series](__GHOST_URL__/baremetal-kubernetes-cluster-start-to-finish/) on creating a Kubernetes cluster. In this post, we'll be creating the virtual machines and preparing them to install Kubernetes (k8s).

For my cluster, we'll be using four Debian 10 "buster" servers. These will be hosted on a [VMWare ESXi](https://www.vmware.com/go/get-free-esxi) physical server. One server will be the k8s master node, and the other three will be worker nodes. I had success with Fedora 29 in my previous cluster, and I'll try to highlight any different steps along the way in case you prefer Fedora. You can mix and match distributions in nodes, but you'll probably have fewer issues if you keep your servers consistent.

[Download the Debian 10 ISO](https://www.debian.org/distrib/netinst) for your architecture (likely AMD64), and use it to boot a virtual machine. Create the master node first with 2 vCPU and 4GB memory. We'll be using NFS for the containers' storage needs, so a 10GB hard drive will be sufficient. The worker nodes will have 4 vCPU and 8GB memory with the same 10GB hard drive.

I won't screenshot every step of the install, but there are a few key points to note. When asked for hostname, I suggest a naming strategy that identifies the machines. I used `k8s-master` for the master node and `k8s1`  `k8s2`  `k8s3` for the worker nodes.

After you answer the basic region and user questions, you will be asked to partition the disk. You need to do manual partitioning to ensure there is no swap partition because k8s does not support swap.

{{< figure src="__GHOST_URL__/content/images/2019/11/manual-partitioning.png" caption="Select Manual partitioning" >}}

{{< figure src="__GHOST_URL__/content/images/2019/11/manual-partitioning-2.png" caption="Select your virtual disk" >}}

{{< figure src="__GHOST_URL__/content/images/2019/11/manual-partitioning-3.png" caption="Create an empty partition table" >}}

{{< figure src="__GHOST_URL__/content/images/2019/11/manual-partitioning-4.png" caption="Select your primary partition" >}}

{{< figure src="__GHOST_URL__/content/images/2019/11/manual-partitioning-5.png" caption="Create a new partition" >}}

{{< figure src="__GHOST_URL__/content/images/2019/11/manual-partitioning-6.png" caption="Use all disk space or enter "max"" >}}

{{< figure src="__GHOST_URL__/content/images/2019/11/manual-partitioning-7.png" caption="Create a Primary partition" >}}

{{< figure src="__GHOST_URL__/content/images/2019/11/manual-partitioning-8-1.png" caption="Use the defaults and select "Done setting up the partition"" >}}

{{< figure src="__GHOST_URL__/content/images/2019/11/manual-partitioning-9-1.png" caption="Select "Finish partitioning and write changes to disk"" >}}

{{< figure src="__GHOST_URL__/content/images/2019/11/manual-partitioning-10.png" caption="Confirm you're not using a swap partition" >}}

{{< figure src="__GHOST_URL__/content/images/2019/11/manual-partitioning-11.png" caption="Confirm writing the changes the disk" >}}

Once the base system has been installed, select the minimal installation for this headless server.

{{< figure src="__GHOST_URL__/content/images/2019/11/software-selection.png" caption="Select SSH server and standard system utilities" >}}

{{< figure src="__GHOST_URL__/content/images/2019/11/grub.png" caption="Install GRUB to the boot loader" >}}

{{< figure src="__GHOST_URL__/content/images/2019/11/grub-2.png" caption="Select the virtual disk" >}}

At this point, the install will finish and the machine will reboot. After rebooting, you'll be at a login prompt.

{{< figure src="__GHOST_URL__/content/images/2019/11/login.png" >}}

Before we can continue, we need to login as root and configure `sudo` for our user. Login using the username `root` and the password you selected during install.

At the prompt, enter this command to install `sudo` and add your user to the `sudo` group (replace `<username>` with the username you chose during installation).

`apt update && apt install -y sudo && usermod -aG sudo <username>`

Lastly, we need to know the IP address of this server. Type `ip addr` to find the IP address. _Note: If you want to control the IP address via DHCP reservation, now is a good time to configure that and reboot._

{{< figure src="__GHOST_URL__/content/images/2019/11/ipaddr.png" caption="Find the IP address for your network card" >}}

Type `exit` to logout.

From now on, we'll use `ssh` to login to the server with the username you created earlier. If your workstation/laptop has Linux, you should have `ssh` already, but if you're on Windows you'll want to [download Putty](https://www.chiark.greenend.org.uk/~sgtatham/putty/latest.html).

{{< figure src="__GHOST_URL__/content/images/2019/11/putty.png" caption="Enter the IP address and click Open" >}}

Check that you can login to the server with the username that you configured earlier (not root). For convenience, you may wish to [setup public key authentication](https://www.digitalocean.com/community/tutorials/how-to-set-up-ssh-keys--2). We will use these `ssh` sessions to manage the servers for the rest of the article.

Repeat the steps above for each of the virtual machines that will belong to the cluster. (k8s1, k8s2, k8s3).

Now that we have some servers, it's time to [install Kubernetes](__GHOST_URL__/baremetal-kubernetes-cluster-installing-kubernetes/).



