---
date: "2015-10-30T14:09:37Z"
description: ""
draft: false
slug: fixed-visual-studio-fails-to-start
title: 'Fixed: Visual Studio Fails to Start'
---


I have had a recurring problem on my PC for a while where after some period of time Visual Studio will fail to start and show a generic error message box. I have multiple versions of Visual Studio and all would fail to load, none telling me exactly what the problem was. I experienced a similar problem trying to start SQL Server Management Studio. Every time this happened, the only thing that could fix it was a reboot. I had tried a few times to search online for similar problems with Visual Studio, but could find nothing useful. Not knowing where to start in troubleshooting, I settled with rebooting occasionally.

Recently when this problem occurred, I happened to try to start [Paint.Net](http://www.getpaint.net) as well, and it also failed to load. It, however, give me a much better error with a call stack. It reported a [TypeInitializationException](https://msdn.microsoft.com/en-us/library/system.typeinitializationexception.aspx) coming from the Microsoft internal static class FontCache.Util. The inner exception indicated a [UriFormatException](https://msdn.microsoft.com/en-us/library/system.UriFormatException.aspx). Using Microsoft’s wonderful code reference site, I pulled up [the code for FontCache.Util](http://referencesource.microsoft.com/#PresentationCore/Core/CSharp/MS/Internal/FontCache/FontCacheUtil.cs) and examined the constructor. In doing so, I found that the code does indeed try to create a Uri based on some string value built using environment variables.

I started up a fresh instance of [LinqPad](http://www.linqpad.net) and pasted the code from the Microsoft site and ran it to see what happens. Sure enough, I experienced the same exception. I inspected a few variables and found that the environment varible “WinDir” was coming back as null. Finally something to start researching. I ended up finding [this Superuser question](http://superuser.com/questions/237268/missing-environment-variables-in-windows-7/488405) about environment variables that stop working after a while and seemed to have put together the final piece of the puzzle. According to the posted answer, having a very long PATH variable can cause the WinDir variable to fail. I did have a long PATH, so I deleted a bunch of things that didn’t seem necessary and my problem was resolved! Thank goodness for Paint.Net showing the real error I would have never known where to start.

I hope that writing this post will help someone else searching the internet trying to find out why Visual Studio is not starting. I couldn’t make the connection online, but hopefully this post can help fix that.

