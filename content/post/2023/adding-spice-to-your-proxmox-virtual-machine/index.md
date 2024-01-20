---
date: "2023-09-27T03:23:52Z"
description: ""
draft: false
slug: adding-spice-to-your-proxmox-virtual-machine
summary: We may finally have a worthwhile alternative to Windows RDP on Linux. Enter
  SPICE!
title: Adding SPICE to Your Proxmox Virtual Machine
---


If there's one thing that Windows indisputably has done better than Linux, it's remote access. Historically, nothing on Linux has worked as well as Microsoft Remote Desktop. VNC has never been a great experience, in my opinion, but if you wanted to run a VM with a Linux graphical desktop environment, it was the default option. This is especially true if your virtual environment is Proxmox.

However, there may finally be a Linux remote access option that can rival the user experience of RDP. It's time to [add some SPICE](https://pve.proxmox.com/wiki/SPICE) to your Proxmox virtual machines!

While Spice appears to be supported by Proxmox out of the box, it is really not much more than a shortcut. You will need to add the drivers on the virtual machine, update the Proxmox VM settings, and install a client application on the client machine.

I'm going to walk you through you how to get this working using a Manjaro Linux VM. The VM is a fresh install of Manjaro 23 with pretty standard Proxmox settings. Let's go!

First, install the SPICE agent on the virtual machine.

```bash
sudo pamac install spice-vdagent
```

Then shutdown the VM and change the hardware setting for the "Display" hardware tos select SPICE for "Graphic card" and enter a value for "Memory (MiB)". The Proxmox docs say "32 MiB is plenty for 4K resolutions."

{{< figure src="image.png" caption="Select SPICE" >}}

Now start the VM and you are ready to connect. But first, you need a client app. On your client machine, install `virt-viewer`. For me, using Ubuntu, it would be

```bash
sudo apt -y install virt-viewer
```

To connect, you need a connection profile file, which you can get from Proxmox. Simply view the VM console in the Proxmox web console and select "Spice".

{{< figure src="image-1.png" >}}

That will download a file that will open using `virt-viewer` and instantly connect you to the VM. And this time you will notice a substantial improvement in graphics, responsiveness, and general user experience. The screen will even dynamically resize to fit your viewer window. (Note: I only found this to be true using the Gnome version of Manjaro. The Budgie desktop was not so kind). In fact, it will almost feel like you are sitting in front of the real computer. If you just put it in full-screen ...

Oh wait, what happened? You may notice the wonderful resizing of the desktop to match the client seems to have broken with full screen. I was able to resolve this by installing the `qemu-guest-agent` on the VM and rebooting.

```bash
sudo pamac install qemu-guest-agent
```

Now when I enter full screen I have a responsive, full screen, beautiful display that feels very comfortable to use. The client viewer was even smart enough to capture key combinations and send them to the VM so my normal shortcuts worked as expected. I was not able to get audio to come through, and video was pretty choppy, but I didn't try to fix those for now.

Happy virtualizing!

