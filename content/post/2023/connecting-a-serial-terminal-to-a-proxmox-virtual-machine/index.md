---
date: "2023-08-20T02:32:55Z"
description: ""
draft: false
slug: connecting-a-serial-terminal-to-a-proxmox-virtual-machine
title: Connecting a Serial Terminal to a Proxmox Virtual Machine
---


When connecting to Virtual Machines or Linux servers in general, `ssh` is the obvious standard in remote terminals. However, if you need to monitor boot output, connect to a machine without a working network connection, or keep a virtual machine [air-gapped](https://www.thesslstore.com/blog/air-gapped-computer/) then a serial terminal may be just the answer you are looking for.

In this article, we will explore how to setup serial terminal access to a virtual machine running on a Proxmox VE host and connect to it without the need for an `ssh` connection or with the limitations of the the Proxmox `VNC` interface. This example VM is running Ubuntu 22.04 but the instructions should be similar for most Linux distributions using grub.

#### Determine VM ID and Proxmox Host

In the Proxmox web admin, find the target VM and note its VM ID and Proxmox host.

{{< figure src="__GHOST_URL__/content/images/2023/08/image-2.png" caption="In this example, the VM ID is 118 and the Proxmox host is pve4" >}}

#### Edit the VM grub config

Now we need to modify the target VM grub config to connect to the serial terminal on boot. This is the only time we will need `VNC` or `ssh` access to the machine.

On the target VM, edit the grub config file.`sudoedit /etc/default/grub`

Update the line shown below:`GRUB_CMDLINE_LINUX="quiet console=tty0 console=ttyS0,115200"`

Save the file and close the editor. For `nano` press `Ctrl+X` then `y` then `[Enter]`.

Reconfigure grub.`sudo update-grub`

Shutdown the VM.`sudo shutdown now`

#### Add the Serial Port to the VM

On the Proxmox host for the VM you are editing, add the serial hardware using the following command:`qm set 118 -serial0 socket`

You can confirm the hardware was added by checking the _Hardware_ settings for the VM. _Note: If the Serial Port shows in red then you need to shutdown the VM to let the hardware change apply. A simple reboot is not enough, which is why the instructions above said to shutdown the VM._

{{< figure src="__GHOST_URL__/content/images/2023/08/image.png" >}}

#### Start the VM and Connect to the Serial Terminal

On the Proxmox host, start the VM and connect to the serial terminal using the below command:`qm start 118 && qm terminal 118`

You will see the boot output and eventually the Linux login prompt. You can login like normal and run the commands as if sitting at the actual machine terminal with a keyboard attached.

Note: If you start the `qm terminal 104` command after boot, it will appear blank because nothing has been written to the serial console. Press `[Enter]` a couple of times and you will see the Linux login prompt to confirm you are connected.

{{< figure src="__GHOST_URL__/content/images/2023/08/image-1.png" caption="Press [Enter] to cause the serial terminal to receive output" >}}

Press `Ctrl+o` to end the terminal session. (That's O as in Oval, not 0 as in zero).

#### Congratulations!

You can now connect to the VM via a virtual terminal any time without needing an SSH session or active network on the VM.

