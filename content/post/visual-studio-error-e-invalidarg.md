---
date: "2010-06-18T12:00:00Z"
description: ""
draft: false
slug: visual-studio-error-e-invalidarg
title: Visual Studio Error E_INVALIDARG
---


I was trying to run some unit tests using MSTest but it could not load the test assemblies, reporting error code E_INVALIDARG. Google pointed me to a bunch of people talking about an ASP.Net web exception and clearing ASP.Net temporary files. Needless to say, this did not fix my problem since I am simply trying to run tests.

After repairing Visual Studio and exhausting all other efforts, I realized that the only related thing I had done lately was install Gallio to try out its test runner and MbUnit. On a long shot I uninstalled Gallio and bazinga my problem was fixed. I don’t know why, but I suspect it has something to do with the Gallio support for MSTest and/or some GAC’ed assemblies from Gallio. I’m just glad to have it fixed regardless of the cause.

Hopefully anyone else having this problem can find this post because there is NO other information online that I could find about this.

