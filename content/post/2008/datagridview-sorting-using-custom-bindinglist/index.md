---
date: "2008-06-05T17:06:00Z"
description: ""
draft: false
slug: datagridview-sorting-using-custom-bindinglist
title: DataGridView Sorting Using Custom BindingList<T>
---


A while back I needed to solve the problem of making all of the `DataGridView` controls in our application sortable, but without needing to implement a custom sort in every single form. The problem is that the `DataGridView`, for reasons unknown to me, does not include default sorting. The only way I could find to get automatic sorting was if the data source was a `DataSet` or if the columns and rows are added manually rather than using a data source. I toyed with the idea of manually adding columns and rows, but that did not last long as I realized that would be entirely too tedious and prone to break. I was also not able to use a `DataSet` as my data source.

Through some research, I did find that when bound to a `BindingList`, the `DataGridView` checks the `BindingList.SupportsSortingCore` property. This is normally `false`, but if you create your own `BindingList` (using inheritance) and override the sort-specific properties, you can implement sorting and the `DataGridView` will take care of the rest.

Let me briefly explain how the `DataGridView` sorting works in relation to the `BindingList`. The `DataGridView` calls the `BindingList.ApplySortCore` method, passing a `PropertyDescriptor` for the sort column and the sort direction. Internally, you’ll use the `List<T>.Sort()` method to handle the sort algorithm, but it is up to you for the item comparison logic. The `DataGridView` will then display the sorted data along with a sorting glyph indicating the sort direction. The sorting glyph has no relation with the data; it is simply tracking internally by the `DataGridView`.

This leaves you with two options regarding the implementation of the sorting. You can accomplish sorting using some generic code that tries to compare the properties using the `IComparable` interface for the property value. Alternatively, you can create a custom sorter for every type of object that you might have sorted in a grid and provide that custom sorter to the `BindingList`. Let me explain the benefits and drawbacks of each method.

######Generic Sort Comparer

* Benefits
 * Reusable
 * Automatically covers any new data types
* Drawbacks
 * Does not compare types that do not implement `IComparable`
 * Not as flexible for complex sorting algorithm

######Type-Specific Sort Comparer

* Benefits
 * Supports advanced sorting and comparison of complex data types
 * Supports sorting of types which do not implement `IComparable`
* Drawbacks
 * Must implement a new sort comparer for each data type to be sorted
 * Must maintain the sort comparer as the class changes

I ultimately decided to use a combination of the two methods. To start with, small data sets that don’t need any complex sorting just use the generic sort comparer, which is the default sort comparer in my custom BindingList. However, I do provide the ability for the `BindingList` to be provided an `ISortComparer<T>` to be used in place of the generic sort comparer. For large data sets or complex comparisons, I provide the `BindingList` with a custom sort comparer and performance improves tremendously.

I’ve uploaded the source files for the concepts mentioned above to gist.github.com.

<script src="https://gist.github.com/jsmarble/a5bf0ededd2df968ea189fea81233472.js"></script>

