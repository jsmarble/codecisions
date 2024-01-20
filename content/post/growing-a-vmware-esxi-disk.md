+++
date = 2022-04-09T20:20:06Z
description = ""
draft = false
slug = "growing-a-vmware-esxi-disk"
title = "Growing a VMWare ESXi Disk"

+++


When working with virtual machines, it is a balance between over-provisioning and under-provisioning resources for each VM. Sometimes, a smaller disk that was fine for months or years is now not sufficient. How can you make the disk bigger without having to start all over?

In this post, I will show you exactly how to expand a disk on the free version of VMWare ESXi 6.7 for a VM running Debian with _ext4_. Some guides show steps for _LVM_ where you create a new partition and just combine them together. This guide will truly expand the partition size and not use _LVM_ at all.

This guide assumes a working knowledge of VMWare ESXi, ssh, linux, and disk partitions.

Before starting, make sure your VM is stopped, has no snapshots, and the disks are consolidated. The command to grow the virtual disk will not work without this step.

Enable SSH on your ESXi host and connect to the console over _ssh_. Change your path to the folder containing the target VM.

```bash
cd /vmfs/volumes/<DATASTORE_NAME>/<VM_NAME>/
```

Find the disk file name, usually something like _VM_NAME.vmdk_. Grow the disk using the following command, specifying your desired FINAL disk size (not the additional disk size). This example will grow the disk to 25GB.

```bash
vmkfstools -X 25g VM_NAME.vmdk
```

If it was successful, you should see the output _Grow: 100% done._ Now start the VM and open a terminal to it.

Next we will grow the partition size to the new disk size. You will need a _root_ terminal and the _parted_ utility installed.

```bash
sudo -i
apt -y install parted
parted
```

We will use _parted_ to verify the new disk size and change the partition size to the new disk size. The _print_ command will show the disk and the partitions.

```parted
print
```

The output will look something like this. You can verify the new disk size with the old partition size.

```parted
Model: VMware Virtual disk (scsi)
Disk /dev/sda: 26.8GB
Sector size (logical/physical): 512B/512B
Partition Table: msdos
Disk Flags:

Number  Start   End     Size    Type     File system  Flags
 1      1049kB  10.7GB  10.7GB  primary  ext4         boot
```

Next we will resize the partition. The number 1 corresponds to the partition number in the output above.

```parted
resizepart 1
```

After confirming, you will be prompted for the end of the new partition. To fill the disk, enter the new disk size. If you specify too large, it will simply give you an error. I will enter the value _25GB_. Then enter the command _q_ to quit _parted_.

```parted
End?  [10.7GB]? 25GB
q
```

Back at the root terminal, we now just need to grow the filesystem to match the partition size. This can be done with one simple command. Substitute the device as necessary. By default, _resize2fs_ will grow the filesystem to the full partition.

```bash
resize2fs /dev/sda1
```

If successful, you should see output similar to the following.

```bash
resize2fs 1.44.5 (15-Dec-2018)
Filesystem at /dev/sda1 is mounted on /; on-line resizing required
old_desc_blocks = 2, new_desc_blocks = 3
The filesystem on /dev/sda1 is now 6103259 (4k) blocks long.
```

Congratulations! You are done and the disk is now bigger. You can verify the free disk space using the _df_ command.

```bash
df -h /

Filesystem      Size  Used Avail Use% Mounted on
/dev/sda1        23G  8.2G   14G  38% /
```

For security reasons, you may wish to disable _ssh_ access to the VMWare ESXi host. I hope this was helpful!

