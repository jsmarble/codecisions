---
date: "2008-11-24T16:21:00Z"
description: ""
draft: false
slug: improvements-to-generic-sort-comparisons
title: Improvements to Generic Sort Comparisons
---


I have [written previously](http://www.codecisions.com/datagridview-sorting-using-custom-bindinglist/) about my solution for providing sorting support to the `DataGridView` using a custom `BindingList<T>` and an implementation of my own `ISortComparer<T>` interface. In that solution, I had provided logic for trying to compare a few simple types generically using reflection. However, a coworker recently pointed out that trying to cast the property values to `IComparable` would allow for a more robust comparison that does not rely in switching the object’s type. I think this is a much more elegant solution and provides support for more data types as long as they support `IComparable`, which many basic framework types do.

I’ve updated the gists from previous post, but I wanted to go ahead and post about the new method. The updated `GenericSortComparer` contains the updates from this post.

<script src="https://gist.github.com/jsmarble/a5bf0ededd2df968ea189fea81233472.js"></script>

