---
date: "2011-09-22T12:00:00Z"
description: ""
draft: false
slug: error-calling-xmlwriter-writevalue-with-guid-parameter
title: Error Calling XmlWriter WriteValue With Guid Parameter
---


Calling `XmlWriter.WriteValue()` passing a `Guid` as the parameter throws an `exception`.

```
StringWriter stringWriter = new StringWriter();
XmlTextWriter writer = new XmlTextWriter(stringWriter);
writer.WriteValue(Guid.NewGuid());
```

The exception thrown is:

```
InvalidCastException: Xml type 'List of xdt:untypedAtomic' does not support a conversion from Clr type 'Guid' to Clr type 'String'.
```

I don’t see any way around this except by casting the `Guid` to a `string` before passing to `WriteValue()`. Since this overload accepts an object type, even an extension method taking a `Guid` doesn’t help because the instance method is the one preferred by the compiler.

Casting the `Guid` to `string` seems to be the only option.

```
writer.WriteValue(XmlConvert.ToString(Guid.NewGuid()));
```

`XmlConvert.ToString()` has an overload that accepts a `Guid` parameter, so we know that will work correctly. The real question is why does `XmlWriter` not just handle a `Guid` parameter itself?

