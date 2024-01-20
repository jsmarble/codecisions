---
date: "2011-01-31T14:00:00Z"
description: ""
draft: false
slug: viewing-exceptions-in-the-immediate-window
title: Viewing Exceptions In The Immediate Window
---


I showed a coworker the other day how you can view exception objects even when you don’t have an exception variable. For those that don’t know this, it is a useful debugging tip to remember.

When handling exceptions, you can use the automatically declared $exception variable in the immediate window or watch windows to access properties of the most recent exception. This can be a handy tip when you need a call stack or any exception type and don’t know from the code. Just type ‘$exception.StackTrace’ into the immediate window and view the output.

