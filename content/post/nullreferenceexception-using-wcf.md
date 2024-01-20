+++
date = 2009-03-26T15:00:00Z
description = ""
draft = false
slug = "nullreferenceexception-using-wcf"
title = "NullReferenceException Using WCF"

+++


I was writing some WCF stub code creating web services and came across a strange NullReferenceException. I could tell from the call stack that it was something to do with the serialization, but couldn’t determine much else. Since it’s coming from the service side, I had real trouble tracking it down. I ended up having to brute force attack it by commenting code until the problem disappeared and then trying to figure it out. It turns out that my problem was caused by exposing a property of my returned class as an interface instead of a concrete instance. I was returning IEnumerable<T>, but when I changed it to List<T> it worked fine. I even tried IList<T> to test my theory, but it failed as well.

My guess is that this behavior has something to do with xml serialization and the inability to create a new instance of an interface without a concrete backing class. This seems to be a drawback in general as I prefer to have publicly exposed members as interfaces, but at least I got it working.

If anyone has any better explanation or helpful advice regarding my observation, please leave me a comment.

