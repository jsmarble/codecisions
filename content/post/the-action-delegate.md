+++
date = 2008-11-06T19:06:00Z
description = ""
draft = false
slug = "the-action-delegate"
title = "The Action Delegate"

+++


If you do any asynchronous work in Windows Forms, you’ve probably used the [MethodInvoker](http://msdn.microsoft.com/en-us/library/bwabdf9z.aspx) delegate a fair amount to invoke a method asynchronously. It’s convenient to have a built-in framework delegate so you aren’t defining your own everywhere. But what about when you’re not in the System.Windows.Forms namespace?

In the past, I had simply defined my own delegate with no parameters and used it. But why create your own simple delegate everywhere when you don’t need any parameters or other customizations on the delegate? In case you didn’t realize, the beloved [System.Action<T>](http://msdn.microsoft.com/en-us/library/018hxwa8.aspx) delegate, which is often used in the [List<T>](http://msdn.microsoft.com/en-us/library/6sh2ey19.aspx).[ForEach()](http://msdn.microsoft.com/en-us/library/bwabdf9z.aspx) method, has a non-generic counterpart, [System.Action](http://msdn.microsoft.com/en-us/library/system.action.aspx), with no parameters or return type. It’s basically a framework version of the simple delegates you’ve been creating.

So, the only question left is, what’s the use of the MethodInvoker delegate in the System.Windows.Forms namespace if the System.Action delegate exists? Is it left-over for compatibility or is there a difference? Since they’re both just delegates, I can’t imagine there’s any benefit to using MethodInvoker.

Now, go forth and write responsive applications without defining useless delegates everywhere.

