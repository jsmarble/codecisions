+++
date = 2016-12-08T01:00:07Z
description = ""
draft = false
slug = "migrating-net-core-preview-2-to-preview-3"
title = "Migrating .Net Core Preview 2 to Preview 3"

+++


If you have any `.Net Core Preview 2` applications that use `project.json`, you will encounter errors if you try to update to `.Net Core Preview 3`. This is due to the fact that `.Net Core` is moving away from `project.json` and will use the normal `.csproj` files going forward. For more information on that change, see [this blog post](https://blogs.msdn.microsoft.com/dotnet/2016/05/23/changes-to-project-json/) by the `.Net Core` engineering team.

Fortunately, the new `dotnet` command line utility supports migrating from `project.json` to the new `.csproj` format. However, that is not very apparent from viewing the command line help output, which normally tells you the commands available to run.

![dotnet help](https://static.codecisions.com/dotnet.png)

But rest assured, the command does exist, and Microsoft even [documented it](https://docs.microsoft.com/en-us/dotnet/articles/core/preview3/tools/dotnet-migrate). I tested it out a simple console application that I had generated with Preview 2, and it worked just as expected. It also backs up your old `project.json` file in case you need it.

