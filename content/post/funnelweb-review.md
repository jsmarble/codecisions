+++
date = 2013-04-12T15:42:29Z
description = ""
draft = false
slug = "funnelweb-review"
title = "FunnelWeb Review"

+++


In my [Blog Refresh](http://www.codecisions.com/blog-refresh/ "Blog Refresh") post I talked about moving back to [BlogEngine.Net](http://www.dotnetblogengine.net) on Arvixe. I used BlogEngine.net for a while but wanted to update to a new version. I had experienced some issues previously with BlogEngine.net upgrades, but always managed to get through it. This time, however, I could not get around my problem (sadly I don’t recall what the problem was since it was years ago).

It’s in my nature to tinker with things, and knowing I was stuck with my existing older version wasn’t appealing. So I embraced my tinkering nature and decided to just try another blog platform out. I wanted to stick with an ASP.Net platform but the only big one I knew of was [dasBlog](http://dasblog.codeplex.com/). I know people who use it, but it’s never appealed to me for some reason. I remembered liking [Paul Stovell’s blog](http://paulstovell.com/blog) so I checked it out and found out he wrote it and published the project called [FunnelWeb](http://funnelweblog.com/).

Since it’s based on ASP.Net MVC and looked pretty slick, I decided to try FunnelWeb out. It even imported my old posts from my BlogML backup of BlogEngine.net. I was happy with FunnelWeb while I used it. It was definitely basic and lightweight, but that minimalism can be appealing. I had posts and comments with email notifications. It supported Akismet spam filtering and had an RSS feed. FunnelWeb seemed to be an active project when I installed it, but there were few updates released. Sadly the project appears to have stalled mostly now. It did get moved over to GitHub recently, but I didn’t see any updates.

The reason [I said it was a mistake](http://www.codecisions.com/blog-refresh/ "Blog Refresh") was because I should have gone with a more seasoned and mature project like dasBlog or something that had better features and a bigger community. Also, while FunnelWeb was eager to import your data, there was no way to export it. I believe it should have provided a way to download the posts in the same BlogML format that it can read. When I decided to refresh my blog and change to Gandi, I had to face the task of how to get my data into WordPress. I know I could have tried to write some extension or feature in FunnelWeb to export data, but I wasn’t as interested in developing for FunnelWeb as I was in just getting my data.

Fortunately, I was successful in writing code to migrate my blog to WordPress. It wasn’t seamless, but it worked. I am going to document the whole process in an upcoming post so anyone interested in moving from FunnelWeb to WordPress should be able to do so with my code and a little effort.

