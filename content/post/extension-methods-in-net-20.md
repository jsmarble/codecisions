+++
date = 2008-10-02T20:34:00Z
description = ""
draft = false
slug = "extension-methods-in-net-20"
title = "Extension Methods in .Net 2.0"

+++


I know I’m late with the news here, but I sure never heard of this until now. Did you know you can do [extension methods](http://msdn.microsoft.com/en-us/library/bb383977.aspx) in .Net 2.0 with a minor hack? I think Visual Studio 2008 might be required. All you have to do, as I learned from [jaredpar’s blog](http://blogs.msdn.com/jaredpar/archive/2007/11/16/extension-methods-without-3-5-framework.aspx), is create your own class called “ExtensionAttribute” in the correct namespace. After that, you have extension methods. That is crazy! I’ve been wanting them for a while, so I’m glad I searched around for this!

As an example, I created an extension method for List<t> called “DoYourThing” that takes an action. This is purely for demonstration and is obviously useless.</t>

```
namespace System.Runtime.CompilerServices 
{ 
    public class ExtensionAttribute : Attribute 
    { 
    } 
}

namespace SampleProject 
{ 
    public static class Extensions 
    { 
        public static void DoYourThing<T>(this List<T> list, Action<T> action) 
        { 
            list.ForEach(action); 
        } 
    } 
}```

If you are unfamiliar with extension methods, get with the gravy train here because they are super cool! No kidding, they are actually really useful.

