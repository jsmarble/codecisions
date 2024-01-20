---
date: "2016-10-19T23:53:00Z"
description: ""
draft: false
slug: nuget-package-costura-fody
title: 'NuGet Package: Costura.Fody'
---


I was writing a small tool recently and wanted to give it to some people in the office to use, but I wanted it to be simple. I was going to put it out on a network share, but I didn't like how I needed a folder with all the dependencies, which inevitably raises questions of which file to run. I thought it would be nice to just bundle it all up into a single executable.

I remembered from years ago that a coworker had done that once before using [ILMerge](https://www.microsoft.com/en-us/download/details.aspx?id=17630), but I had no idea how he did it. I figured it couldn't be that hard, so I went to searching for the answer. It didn't take long before I figured out that I didn't want to go that route. The command line arguments are a pain when you have a ton of dependencies, and it sounded like it was going to be more trouble that it was worth.

Fortunately, while browsing around, I found mention of a Nuget package called [Costura.Fody](https://github.com/Fody/Costura) that claimed to be able to achieve the same thing automatically! You can read the details on their [GitHub project page](https://github.com/Fody/Costura) but just rest assured that it really is as easy as they say it is.

I added the Nuget package, built my project, and opened my bin folder. I found everything looking the same as before apart from the main executable being much larger. I copied the executable to a folder by itself, and to my delight it ran without a hitch.

I think this is a great way to go for distributing small utilities and making things easy for users when you are not creating a formal installer for an application. I love finding Nuget packages that help so much with so little fuss.

