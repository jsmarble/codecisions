---
date: "2009-12-23T14:00:00Z"
description: ""
draft: false
slug: filter-stacking-linq-expressions
title: Filter Stacking LINQ Expressions
---


Recently I was attempting to devise a way to filter down a large list with user generated conditions. These could be handled different ways depending on user input, so I couldn’t use a predefined lambda expression to filter the collection. What I wanted was a way to build an unknown number of lambda expressions from the user input and use them to filter the collection.

I ended up parsing the user input and adding each search expression to a List<func bool="">>. Then after I had built all of my lambda expressions, I iterated them, filtering the list smaller and smaller each time. In the end I had a nicely (and quickly) filtered list and the support for an unlimited amount of search logic. The only limitation was how to parse the user’s input into a lambda expression.</func>

```
List<TSource> sourceList; //Your source list to search.
List<Func<TSource, bool>> expressions = new List<Func<TSource, bool>>();
//Build expressions from user input and add them to the expressions list.
IEnumerable<TSource> results = sourceList;
foreach (var expression in expressions)
    results = results.Where(expression);
return results;
```

