+++
date = 2011-10-18T12:00:00Z
description = ""
draft = false
slug = "converting-hash-byte-array-to-string-output"
title = "Converting Hash Byte Array To String Output"

+++


Years ago, when I was learning how to use C# to calculate hash values, I learned that to properly output the byte array of a hash (or any other byte array) you had to iterate each byte and call byte.ToString(“X2”) method for the proper formatting, adding each byte string to a StringBuilder.

```
StringBuilder hash = new StringBuilder();
foreach (byte b in hashBytes)
    hash.Append(b.ToString("X2"));
```

Output:

```
DC724AF18FBDD4E59189F5FE768A5F8311527050
```

Not long ago I was doing something similar and realized that you could use Linq’s Aggregate operator for the same thing in a more concise syntax.

```
string hash = hashBytes.Aggregate (new StringBuilder(), (sb,b)=>sb.Append(b.ToString("X2") ), sb=>sb.ToString());
```

But today I was reading the [MSDN documentation on BitConverter](http://msdn.microsoft.com/en-us/library/system.bitconverter.aspx) and saw that there is a method to convert a byte array to a string. I tried it out to see how it compared to the output I was familiar with and saw it was the same except with dashes in between each byte string.

```
string hash = BitConverter.ToString(hashBytes);
```

Outputs:

```
DC-72-4A-F1-8F-BD-D4-E5-91-89-F5-FE-76-8A-5F-83-11-52-70-50
```

Of course, if you don’t want the dashes, it seems the easiest thing to do would be to replace all dashes with null.

```
string hash = BitConverter.ToString(hashBytes).Replace("-", null);
```

