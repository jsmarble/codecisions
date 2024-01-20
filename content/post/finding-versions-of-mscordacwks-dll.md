+++
date = 2014-11-24T16:48:41Z
description = ""
draft = false
slug = "finding-versions-of-mscordacwks-dll"
title = "Finding Versions of mscordacwks.dll"

+++


In my post on [Debugging Different SOS Versions With WinDBG](http://www.codecisions.com/debugging-different-sos-versions-with-windbg/), I wrote how you often will need a different version of mscordacwks.dll, but I skipped over the part about how to obtain the version you need.

Of course the natural answer is to get it off the machine from which the dump file was obtained. Unfortunately, that is often easier said than done. I have previously looked online to try to find copies with little luck, but today I ran across a site that attempts to index every version of the file.

http://sos.debugging.wellisolutions.de/

There are folders for x86 and x64, along with subfolders for various versions. So, next time you are debugging a dump file and need mscordacwks.dll or SOS.dll, check them out. I found what I needed, but if they are missing your version, you might considering sending it to them once you find it.

