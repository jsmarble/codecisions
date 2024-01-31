---
date: "2024-01-31"
description: ""
draft: false
slug: a-static-blog-with-hugo-and-github-pages
title: A Static Blog with Hugo and GitHub Pages
---

For years now I have operated this blog using Ghost running as a docker container. That has served me well, and it's en excellent system, but I felt it was time for a change. Back when I switched to Ghost, there was not much talk of static site generators or free static hosting platforms. Containers were huge and cool (and still are), and it made sense to run my own ghost server. However, after years of dealing with version upgrades, container image tags, database management, unintended downtime, etc., I have decided to take a swing at using static site generators and third-party hosting.

The online writing tool space is very capable right now. Markdown has really taken ahold of the writing world from a format perspective and is more widely supported than ever. Combine that with tools that can do a great job of converting markdown into HTML for rendering a website and you have a great process for writing and publishing. Now take the process and put it into a mature automation pipeline like GitHub Actions and have the output sent to a static hosting platform like GitHub Pages and you have one mighty powerful and resilient publishing process, all for free.

It's amazing to me that I can write a blog post in simple markdown using a clean editor (I am using [Typora](https://typora.io) right now), then just commit it to my GitHub repository and have an updated site automatically published in minutes.
