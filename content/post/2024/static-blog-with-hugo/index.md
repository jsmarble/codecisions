---
date: "2024-01-31"
description: ""
draft: false
slug: a-static-blog-with-hugo-and-github-pages
title: A Static Blog with Hugo and GitHub Pages
---

For years now, I have operated this blog using Ghost running as a docker container. That has served me well, and it's an excellent system, but it was time for a change. When I switched to Ghost years ago, there was little talk at that time of static site generators or free static hosting platforms. Containers were huge and cool (and still are), and it made sense to run my own ghost server. However, after years of dealing with version upgrades, container image tags, database management, unintended downtime, etc., I have decided to take a swing at using static site generators and third-party hosting.

The online writing tool space is very capable right now. Markdown has taken ahold of the writing world from a format perspective and is more widely supported than ever. Combine that with tools that can do a great job of converting markdown into HTML for rendering a website, and you have a great process for writing and publishing. Now take that process and put it into a mature automation pipeline like GitHub Actions and have the output sent to a static hosting platform like GitHub Pages, and you have one mighty powerful and resilient publishing process, all for free.

It is incredible that I can write a blog post in simple markdown using a clean editor (I am using [Typora](https://typora.io) right now), then commit it to my GitHub repository and have an updated site automatically published in minutes. To achieve this, I decided to use the static site generator Hugo, which can convert Markdown content into HTML according to a prescriptive structure. I then used GitHub Actions to automate the publishing process whenever a commit is made. It's a very clean system that should be reliable and reproducible for a long time.

If you want to do something similar, this entire site, including the content, pipelines, automation, and everything involved in publishing the site from Markdown to GitHub, is [publicly available on GitHub](https://github.com/jsmarble/codecisions/). Also, Hugo has its own [guide to publishing on GitHub Pages](https://gohugo.io/hosting-and-deployment/hosting-on-github/).

It's an amazing world right now, full of fantastic free tooling. Enjoy it!