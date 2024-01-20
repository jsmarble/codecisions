---
date: "2009-04-15T13:56:00Z"
description: ""
draft: false
slug: tooltip-autopopdelay-failure
title: ToolTip AutoPopDelay Failure
---


Most things in Microsoft’s .Net Framework are pretty intuitive and function as expected, but every so often you run across something that is just plain stupid. I’ve run across multiple problems with tooltips before, including a nasty one in the 1.1 framework in which the tooltip would not redisplay after it timed out and hid. But, alas, this is not what I’m talking about today. Today I want to talk about the frustrating nature of ToolTips and hiding their popup display after five seconds.

Today at work we were asked to extend the time that a tooltip remains visible. The ToolTip’s AutoPopDelay property seems to be the answer. It’s description says “Gets or sets the period of time the ToolTip remains visible if the pointer is stationary on a control with specified ToolTip text.” It could not be much clearer that this is the property I was looking for. However, upon setting this to a large value, such as 60000 (60 seconds), I found that the tooltip still disappeared after five seconds. This seemed like a glaring bug, but figuring that a problem so obvious must have been encountered by the development community already, I did some Googling and found the secret answer.

For reasons unknown to me, the tooltip AutoPopDelay only supports values up to 32767. Any value higher than that and it resets to the default 500. If that value looks familiar, it’s because that value is the constant Int16.MaxValue. This would all be fine and well if AutoPopDelay was exposed as an Int16, but it’s an Int32. Even if the documentation for the property indicated that gotcha, that would at least be something. But no, I had to find an obscure forum post for the answer. Come on Microsoft, you lack in your documentation sometimes, but this glaring omission combined with the fact that the behavior even exists is truly disappointing.

