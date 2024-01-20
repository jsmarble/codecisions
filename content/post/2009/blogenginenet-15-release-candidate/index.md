---
date: "2009-04-07T19:50:00Z"
description: ""
draft: false
slug: blogenginenet-15-release-candidate
title: BlogEngine.Net 1.5 Release Candidate
---


I read [Al Nyveldt’s post](http://www.nyveldt.com/blog/post/BlogEngineNET-15-Release-Candidate-available.aspx) that a BlogEngine.Net 1.5 Release Candidate is available and immediately thought to download it and try it. Since my blog is very low traffic and not critical, I had no concerns about trying out the new version.

I followed Al’s basic [instructions](http://www.nyveldt.com/blog/post/BlogEngineNET-145-Upgrade-Guide.aspx) for upgrading to 1.4.5 and backed up my entire blog before wiping it clean and copying over the new files. Then I simply copied back my App_Data folder and all was well …. except the theme.

I could not get any theme at all. Not the standard theme, not anything. If I appended the query string “?theme=Standard” then it was fine, but by itself nothing worked. I searched the discussions over at the [BlogEngine.Net CodePlex site](http://blogengine.codeplex.com) and found [this discussion](http://blogengine.codeplex.com/Thread/View.aspx?ThreadId=52370) about my exact problem. It turns out that people hosting on IIS7 could need a different <system.webServer> section in their web.config file. I don’t honestly know what the actual fixed part is, but I used the section [linked](http://blogengine.codeplex.com/WorkItem/View.aspx?WorkItemId=9323) in the discussion and everything started working again.

So far it is about the same from the everyday experience, but the [Windows Live Writer](http://writer.live.com/) tag support is nice. Just to be safe, I went to the “Edit blog settings” section of WLW and clicked “Update Account Configuration” to make sure that the new version supported features were recognized by WLW.

