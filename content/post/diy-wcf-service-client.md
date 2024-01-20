---
date: "2008-10-22T20:18:00Z"
description: ""
draft: false
slug: diy-wcf-service-client
title: DIY WCF Service Client
---


When writing WCF services, the “by the book” method of consuming a service is to add a Service Reference to your project and let Visual Studio codegen all the necessary classes including a service client. While this is convenient, it can pose some problems in more complex scenarios. As an example, I’ll walk you through an extremely simple situation using `WCF` Services to get GUIDs.

Let’s say that you have a service interface called `IGuidService`. Now lets say that you have defined a web service that implements this interface along with just a regular class (not a `WCF` Service) which also implements this interface. If you let Visual Studio do the codegen for you, you have another copy of the interface in another namespace. This causes ambiguity in your classes and also throws a kink in your plans for using Dependency Injection (or it did in my sample). So, what you really need is a service client that uses your known interface, but if you change the generated service client code to implement your interface then it might break later when the code is regenerated. So I guess the only option left is to write your own.

Well, if you’re going down that road, you’ll be glad to know that writing your own is, in my opinion, easier than letting Visual Studio handle it. `WCF` still does all the hard work, you just have to inherit a base object. Create a class that inherits `ServiceBase<IGuidService>` and then implement `IGuidService` also and you’re good to go.

I wrote an example project and included it for your viewing pleasure. It also is a basic example of Dependency Injection using [`Ninject`](http://ninject.org/).

[WCFService1.zip](http://static.codecisions.com/WCFService1.zip)

