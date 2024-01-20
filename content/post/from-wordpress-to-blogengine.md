---
date: "2008-07-16T15:49:00Z"
description: ""
draft: false
slug: from-wordpress-to-blogengine
title: From Wordpress To BlogEngine.net
---


If you’ve seen some strange `RSS` activity recently, I apologize. I am wanting to get into `ASP.NET` (specifically the new `MVC` framework) since I don’t do web stuff at work and feel I ought to be better versed in it. My [former web host](http://jkahosting.com) was `Linux/PHP`, which worked fine for WordPress, but I was not really interested in `PHP` programming. So, I signed up for an account with [WebHost4Life](http://www.WebHost4Life.com/default.asp?refid=jsmarble) and at the same time restructured the family website. I decided that I didn’t like the subdomain of blogs.* but instead I wanted to have each family member (currently just myself) have their own subdomain and have the /blog folder setup under each subdomain. Fortunately, the use of FeedBurner allows me to keep the same feed URL for all you RSS subscribers, but unfortunately, I could find no way to prevent the duplication of RSS items after the import.

Speaking of the import, let me tell you how the migration from WordPress to BlogEngine.net went. The standard method for converting blogs is using the [`BlogML`](http://blogml.org) format. The creation of the `BlogML` file with WordPress was extremely easy with [Robert McLaws’ WordPress BlogML export utility](http://www.windows-now.com/blogs/robert/archive/2007/05/07/wordpress-blogml-export-1-0.aspx). I wish I could say the import went as well. I used the [BlogEngine.net BlogML import utility](http://www.codeplex.com/blogimporter/) to try to import the `BlogML` file. It busted first try. It said my username or password was incorrect, even though it wasn’t. Let me summarize by saying that the utility is pretty young, but it does work actually. Thankfully, since I’m a .NET developer, I was able to download the source and step through and get the actual exception that occurred. Turns out, it was an error in the import service that is part of the BlogEngine.net install (`api/BlogImporter.asmx`).

Let me tell you how I found the problem. First I had to narrow down the line in the `asmx` causing the error. Sadly, since it’s on the server, I couldn’t step through it. So I did the old throw exception “here1” and see where I got. Turns out it was happening inside the `[Post].Import()` method. I didn’t want to download all the source to BlogEngine.net and find this class, so I used the trusty [Reflector](http://www.red-gate.com/products/reflector/) and reflected the `BlogEngine.Core.dll` and viewed the source of the offending method. The exception had to do with adding a value to a `DateTime`, so that narrowed down the possible coding culprits. I noticed that two different date properties were being modified, but I also noticed that only one of them had been set by the aforementioned import service. So, I added a line to the import service and bingo, the import worked.

Now the only problem I have is that none of the categories or tags got imported (and were therefore not linked to any posts) and all of the pages were treated as posts. Oh yeah, and I lost all my hard-earned formatting. So now I’m having the fun of going back and reformatting posts and applying tags and categories.

I’ve attached the modified `BlogImporter.asmx` file and the WordPress BlogML export file to this post for your convenience in case you happen to find yourself in the same situation as me.

[BlogEngine Importer](http://static.codecisions.com/BlogEngine-Importer.zip)

