---
date: "2009-09-11T19:46:30Z"
description: ""
draft: false
slug: reading-command-line-arguments
title: Reading Command Line Arguments
---


If you write very many command line utilities, you have likely had to read command line arguments many times. Since this is very much the same thing every time, it seems like a waste to write rigid and app-specific arg parsing to each program.

Instead, I have created some classes to read argument arrays and turn them into something more usable. I have published these classes in a [Github Gist](https://gist.github.com/jsmarble/8d90828b11dc4fd3724c89b148e1d0ca). The first [comment](https://gist.github.com/jsmarble/8d90828b11dc4fd3724c89b148e1d0ca#gistcomment-1762202) on the Gist describes how it works and has usage examples.

<div class="oembed-gist"><script src="https://gist.github.com/jsmarble/8d90828b11dc4fd3724c89b148e1d0ca.js"></script><noscript>View the code on [Gist](https://gist.github.com/jsmarble/8d90828b11dc4fd3724c89b148e1d0ca).</noscript></div>If you include this in a shared assembly, it can be referenced from any console application and you’ll quickly and easily have arguments to work with.

