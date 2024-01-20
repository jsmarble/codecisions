+++
date = 2013-06-21T14:45:41Z
description = ""
draft = false
slug = "controlling-browser-interpretation-of-character-encoding"
title = "Controlling Browser Interpretation of Character Encoding"

+++


This article is about character encoding, but I thought it would be beneficial to give some background on what I’m doing.

I’ve been working on a project that generates a report, and I needed to be able to output my report in any format, including XML and HTML. Through an effort to achieve a good modular design, I wrote several report formatters that all just return a byte array to be persisted (to file primarily, but streams or a db are possibilities). To achieve the HTML, I used an XSLT against my existing XML formatting.

Everything was going fine until I decided to run my report on a Russian language server. When the HTML report launched in IE, the Cyrillic characters were garbled. I knew it was an encoding issue, but I had no idea how to solve it. Time to [brush up](http://coding.smashingmagazine.com/2012/06/06/all-about-unicode-utf8-character-sets/) on character encodings (again).

When I first was trying to see where my problem started, I viewed source from the browser, but that also had the garbled characters. I then tried the XML report, and it opened in IE just fine. At this point, I thought for sure I had narrowed it down to my XSLT transformation logic. Despite trying to [specify the XSLT output encoding](http://devproj20.blogspot.com/2008/02/writing-xml-with-utf-8-encoding-using.html), nothing worked.

I decided to open the temp HTML file directly in Notepad and discovered that it had the correct characters; it was just the browser that displayed them incorrectly. But why could notepad read the characters but the browser could not?

Some more [research](http://en.wikipedia.org/wiki/Character_encodings_in_HTML) pointed me to the fact that browsers have logic to pick a character set if one is not specified. I tried editing my temp HTML file in Notepad to specify the encoding using an [HTTP META tag](http://www.w3schools.com/tags/tag_meta.asp). It worked! Logically, I assumed that if I then deleted my change, it would go back to displaying incorrectly. But when I tried that, it still displayed correctly. Now I was really confused.

I reproduced my bad file again and made a single whitespace change in Notepad and saved the file. Again, it displayed correctly. Now I knew there was something wrong with how I created the file. Some more [research](http://stackoverflow.com/questions/2223882/whats-different-between-utf-8-and-utf-8-without-bom) uncovered the topic of Byte Order Marking (BOM) and how that indicates what character encoding is used in a file. When I was creating my HTML bytes, I was explicitly using UTF8 encoding, so I wasn’t sure why that fact wouldn’t be persisted in the bytes themselves.

`return Encoding.UTF8.GetBytes(s);`

I saw on [MSDN](http://msdn.microsoft.com/en-us/library/ms143375.aspx) that File.WriteAllText() will write the BOM only if you specify a specific encoding in an overload. Encoding.UTF8.GetBytes() does not have any overloads, and the bytes are coming from a specific encoding, so do those bytes include the BOM for that encoding? A [Stack Overflow question](http://stackoverflow.com/questions/420867/why-isnt-the-byte-order-mark-emitted-from-utf8encoding-getbytes) contained my answer. Encoding.UTF8.GetBytes() does not include a BOM, so the file didn’t specify what character encoding it was. When IE saw the page, it tried to look at my content and figure it out, but it was not successful.

Fortuntely, that Stack Overflow article also indicates how to add a BOM to the byte array. Encoding.UTF8.GetPreamble() will get the BOM bytes, then you have to put the character bytes after that.

`return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(s)).ToArray();`

There is argument about whether UTF8 should require a BOM since there is only one UTF8. The official recommendation is that UTF8 should not include a BOM, but that leaves interpretation of the bytes up to the consuming application. The [character encoding article](http://coding.smashingmagazine.com/2012/06/06/all-about-unicode-utf8-character-sets/) I referenced earlier has a section devoted to the different options for interpreting character encoding. Clearly IE’s method did not correctly resolve my UTF8 file, so in the end I decided that specifying the BOM was a reasonable solution.

As for why editing the file in Notepad resolved the problem, I believe saving the file automatically added the BOM for me. I also want to point out that Chrome seemed to interpret my BOM-less file correctly as UTF8, so you never know how an application will choose to interpret your characters if you don’t tell it what encoding was used. In the end I’d rather specify my character encoding than rely on the browser to figure it out.

