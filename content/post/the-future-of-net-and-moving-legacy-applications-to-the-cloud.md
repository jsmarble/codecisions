---
date: "2019-09-30T18:35:43Z"
description: ""
draft: false
slug: the-future-of-net-and-moving-legacy-applications-to-the-cloud
title: The Future of .NET and Moving Legacy Applications to the Cloud
---


The release of .NET Core 3 raises the question of whether there are any impacts to the future direction of legacy .NET Framework applications that may be interested in moving to .NET Core in the future. The short answer is not really, but Core 3 is a stepping stone to getting the full .NET stack into a position where it could affect your ability to adopt the newer framework capabilities.

This latest version of .NET Core marks the final major version to be called .NET "Core" in Microsoft's roadmap. Core 3 [closes much of the remaining capability gap](https://devblogs.microsoft.com/dotnet/introducing-net-5/) with .NET Framework 4.8, enabling Windows Forms, WPF, and Entity Framework 6. This opens the door for Microsoft to merge the two platforms and bring all of .NET into a unified future with a stronger, more modern, and open-source codebase.

The next version of .NET, due out in Fall 2020, will be a combination of the legacy .NET Framework and the modern .NET Core platforms. It remains to be seen exactly how seamless the transition will be to move from .NET Framework 4.X to .NET 5, but let's remain hopeful that Microsoft will make it somewhat painless.

If .NET 5 can be adopted in a reasonable timeframe with moderate effort, this represents a significant opportunity for the applications on the classic .NET Framework. By combining the lightweight, cross-platform nature of .NET Core with the full capabilities of .NET Framework, there exists potential for applications to begin taking advantage of cloud-optimized platform features, such as serverless functions and containers, and lower-cost OS platforms, without the need for rewriting their code to fit a different framework. In reality, it is unlikely that this can achieved without some meaningful development effort, but the degree of that effort should be far less than it would have been up until now.

The good news is that the current strategies for modernizing legacy applications fits perfectly into Microsoft's roadmap. One of the biggest strategies is breaking up a monolith into microservices and independent projects. This can have a significant impact on the adoptability of .NET 5 if smaller or more independent services can be adapted to the new framework for its benefits without affecting more complex systems that may take additional effort. If Microsoft sticks to their timeline for releasing .NET 5, you have approximately a year to get your legacy application broken down into microservices. This will enable you to independently upgrade each one to .NET 5 and strategically migrate your entire application platform in a safe and incremental way.

For the .NET Framework developers out there, does .NET Core 3 even matter for your legacy application? Not directly, no, but the engineer in your spirit is probably interested in what's new (executables in build output is exciting, and "Blazor" deserves some more reading). Also, be sure to try out Visual Studio 2019 along with C# 8 bringing new language features. You can explore the links below, but the highlights include using declarations (convenient), null-coalescing assignment operator (convenient), nullable reference types (intriguing), and default interface members (controversial).

https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0/

https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8

https://devblogs.microsoft.com/dotnet/introducing-net-5/

