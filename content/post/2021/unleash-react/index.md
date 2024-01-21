---
date: "2021-06-23T22:49:28Z"
description: ""
draft: false
slug: unleash-react
title: Unleash React - What is Feature Flagging?
---


[Feature flags](https://www.martinfowler.com/articles/feature-toggles.html) are a powerful tool in the developer's toolbox. Instead of releasing code in large features and risky deployment operations, new features can be developing incrementally and turned on and off in production by toggling a flag, selecting certain users, or other similar strategies. This makes the deployed code less volatile and provides an easy way to rollout and rollback code changes with less impact and risk.

In this series, I am going to introduce you to feature flags and show you from start to finish how to get started with feature flagging in a [React](https://reactjs.org/) application.

This series includes:
- [Intro to Feature Flagging and Unleash](/unleash-react/) (this post)
- [Running the Unleash Server](/unleash-react-running-the-server/)
- [Configuring Unleash Proxy](/unleash-react-configuring-unleash-proxy/)
- [Creating the React App](/unleash-react-create-the-app/)

You can find the full sample application [available in GitLab](https://gitlab.com/codecisions/unleash-react).

---

[Unleash](https://www.getunleash.io/) is one of several frameworks available that developers can use to add feature flags to their applications. Notably, [it is open-source software](https://github.com/Unleash/unleash).

The heart of [the Unleash architecture](https://www.unleash-hosted.com/articles/our-unique-architecture/) is the Unleash Server. It contains all of the feature flags and strategies employed to control the flag state.

In addition to the server, Unleash has [SDK libraries](https://docs.getunleash.io/sdks) for a number of popular languages. They also have lightweight proxy SDKs for client-side apps which I'll touch on later.

---

Now that you have a basic understanding of feature flagging and Unleash, let's [get Unleash running in docker](/unleash-react-running-the-server/).

