---
date: "2012-03-06T14:00:00Z"
description: ""
draft: false
slug: the-flaw-of-linqs-single-extension
title: The Flaw of LINQ's Single Extension
---


The purpose of the `Single` extension methods is to thrown an exception if more than one item matches the query (or, in the parameterless implementation, more than one item exists in the enumerable). It seems obvious then that the method should stop enumerating once finding the second match, throwing the appropriate exception. And indeed this is how the parameterless implementation behaves.

The code in this first example calls `Enumerator.MoveNext()` twice. Once for the matching result, and once to find that a second result exists, throwing the `exception`.

```
<code class="language-c">Enumerable.Range(1,10).Single()
```

However, the code in this second example calls `Enumerator.MoveNext()` 10 times, once for each element in the source enumerable. It counts the number of matches and then still simply throws the `exception` if more than one match exists.

```
Enumerable.Range(1,10).Single(x => x < 5)
```

The counting of the matches is useless, and therein lies the performance problem. If there were many items in the source `IEnumerable` or enumerating the source incurred a performance penalty, it is a waste of time and resources to continue enumerating past the second matching item. Once it is determined there is more than one match, enumeration should cease and the exception should be thrown.

The simple solution to this problem is to change the `Single` method to the Where method with a `Single` method hanging on the end. This combines the `Where` filter with the proper behavior of the parameterless `Single` extension.

```
Enumerable.Range(1,10).Where(x => x < 5).Single()
```

