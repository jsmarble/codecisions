---
date: "2008-10-27T19:19:40Z"
description: ""
draft: false
slug: force-delete-files-or-folders-in-windows
title: Force Delete Files or Folders In Windows
---


I was rearranging some music last week using a [tag scanning program](http://www.xdlab.ru/en/index.htm), but during the process I ended up with an empty folder with a funny name. I went to simply delete the folder, but Windows reported that it could not delete the folder because it was no longer available. First I tried using the command prompt to delete it, but that also failed. So, I naturally rebooted my machine to delete the folder on a fresh reboot. Still, no dice.

This was rather interesting now since Windows Explorer continued to see the folder and even let me open it, but it was rather adamant that it didn’t exist as soon as I tried to work with the folder at all. Next step, Google it. I found a [nice article](http://www.siliconkid.com.au/sk_archive/windows_xp/undeletable_folder.html) which discusses the possibility of a folder getting created with invalid characters (which theoretically shouldn’t be allowed in the first place, but I digress). The article referred me to a program called [DeleteFXPFiles](http://www.jrtwine.com/products/dfxp/index.htm). This seems like a good candidate for a standalone executable, but alas it has an installer. Fortunately I was able to extract the installer with [7-zip](http://7-zip.org) and run it standalone. The app hung at first, but eventually came back and was able to delete my folder.

There may be other applications out there that can do this, but DeleteFXPFiles did it quickly and without complaining.

