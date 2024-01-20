---
date: "2011-02-25T14:00:00Z"
description: ""
draft: false
slug: formatted-xmlwritertracelistener
title: Formatted XmlWriterTraceListener
---


I was wanting the output of the [`XmlWriterTraceListener`](http://msdn.microsoft.com/en-us/library/system.diagnostics.xmlwritertracelistener.aspx) to be formatted, so I figured I could just adjust some settings similar to the [`XmlWriter`](http://msdn.microsoft.com/en-us/library/system.xml.xmlwriter.aspx) class and get formatting, indentation, etc. However, upon viewing the `XmlWriterTraceListener` code in [Reflector](http://www.red-gate.com/products/dotnet-development/reflector/), I found that it writes out its XML using a plain `TextWriter`, which means no intelligent XML formatting. I imagine this is for performance reasons or some other good reason, but if you really want or need formatted log output, you’re out of luck.

I decided the only way to keep the built-in behavior of the `XmlWriterTraceListener` but still allow formatted output was to re-implement the CLR class and override the output to use an `XmlWriter`. Using Reflector, I painstakingly implemented the exact same behavior and logic using an `XmlWriter` for output. My new class, `FormattedXmlWriterTraceListener`, inherits `XmlWriterTraceListener` for consistency and integration with the .Net tracing system. The real strange part was the constructors because I couldn’t pass down the file name or else it would try to start up the base class `TextWriter` logic. Instead, this implementation keeps track of its `XmlWriter` and path information in the instance. Properties have been added to support the various `XmlWriter` indentation and formatting options.

The source for the [`FormattedXmlWriterTraceListener`](https://gist.github.com/jsmarble/1281713a5f82e6cb4bb9561531074606) is available on gist.github.com.

<script src="https://gist.github.com/jsmarble/1281713a5f82e6cb4bb9561531074606.js"></script>

