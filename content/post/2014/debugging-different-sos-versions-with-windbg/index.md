---
date: "2014-01-20T19:23:46Z"
description: ""
draft: false
slug: debugging-different-sos-versions-with-windbg
title: Debugging Different SOS Versions With WinDBG
---


Debugging memory dump files from .Net processes with WinDbg is always an adventure. I have below a solution to the common problem of CLR version mismatching. But first, I will give a quick introduction to WinDbg and memory dumps for those new to the topic.

Memory dumps are files with the contents of the memory of a process. They contain variable data, method calls, exceptions, and anything else. An easy way to create a memory dump is to use Task Manager. Right click on a process and select `Create Dump File`. Note that on x64 machines you must use the x64 task manager if capturing a memory dump from a 64-bit process and vice versa for x86.

Once you have a `.dmp` file, you will open it with WinDbg (included in the Windows SDK Debugging Tools). Again, use the correct architecture (x64/x86) WinDbg to match your dump file. In WinDbg, click `File -> Open Crash Dump` and open the file.

The CLR must be loaded into WinDbg prior to working with CLR debugging commands. Run the command `.loadby SOS CLR` to load the CLR debugging modules. For more information on getting started with WinDbg, see [this blog series](http://blogs.msdn.com/b/johan/archive/2007/11/13/getting-started-with-windbg-part-i.aspx) on MSDN.

By far the most common problem I come across with trying to open dump files is that the CLR version of the computer on which the dump was captured is not exactly the same as on the debugging computer. In that case, WinDbg gives you this error when trying to run CLR commands. The value `CLR Version` is the version from the dump machine and `SOS Version` is the version from the debugging machine. Note that I’ve found that WinDbg sometimes does not give you the CLR and SOS versions the first time the error is displayed. If not shown, simply run the command again to generate the error again, hopefully with the version information included.

```
0:000> !printexception
The version of SOS does not match the version of CLR you are debugging.  Please
load the matching version of SOS for the version of CLR you are debugging.
CLR Version: 4.0.30319.18052
SOS Version: 4.0.30319.18408
Failed to load data access DLL, 0x80004005
Verify that 1) you have a recent build of the debugger (6.2.14 or newer)
            2) the file mscordacwks.dll that matches your version of clr.dll is 
                in the version directory or on the symbol path
            3) or, if you are debugging a dump file, verify that the file 
                mscordacwks_<arch>_<arch>_<version>.dll is on your symbol path.
            4) you are debugging on supported cross platform architecture as 
                the dump file. For example, an ARM dump file must be debugged
                on an X86 or an ARM machine; an AMD64 dump file must be
                debugged on an AMD64 machine.

You can also run the debugger command .cordll to control the debugger's
load of mscordacwks.dll.  .cordll -ve -u -l will do a verbose reload.
If that succeeds, the SOS command should work on retry.

If you are debugging a minidump, you need to make sure that your executable
path is pointing to clr.dll as well.
```

The message is somewhat helpful in that it tells you what versions you’re dealing with and that you need the specific version of `mscordacwks.dll` in a specified naming format. It does not, however, tell you where to put this file or that it will not entirely solve this error. I have found that you actually need a second file, `SOS.dll`, alongside `mscordacwks.dll`. I know many people say you only need `mscordacwks.dll` but I have had situations where that file alone did not resolve the problem but adding in `SOS.dll` did.

To fix this, I copied `SOS.dll` and `mscordacwks.dll` from the dump machine to my debugging machine, then renamed them with `_AMD64_AMD64_4.0.30319.18052` appended to the name.

```
mscordacwks_AMD64_AMD64_4.0.30319.18052.dll
SOS_AMD64_AMD64_4.0.30319.18052.dll
```

I then put these files into the same folder as `WinDbg.exe`.

Once the files are in place, either restart WinDbg or run the command `.cordll -u -l` to reload the CLR debugging modules. Now you can run CLR commands without issue.

