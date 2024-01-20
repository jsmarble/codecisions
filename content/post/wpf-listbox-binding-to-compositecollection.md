---
date: "2011-09-01T12:00:00Z"
description: ""
draft: false
slug: wpf-listbox-binding-to-compositecollection
title: WPF ListBox Binding to CompositeCollection
---


I was using ListBox to display data bound to a collection, but I also wanted to be able to have an Add New item at the bottom of the list that the user could click to add a new item to the end of the list.

Coming from a simplified Windows Forms binding background, my first thought was to just modify the source collection to include my placeholder item. This worked fine functionally, but it caused the collection to not be accurate and caused extra work for me to add and remove the placeholder at certain times. Also, I was binding another display field to the collection count property, and therefore it was always one higher than the true count.

After doing some research, I discovered that a very useful collection called the [CompositeCollection](http://msdn.microsoft.com/en-us/library/system.windows.data.compositecollection.aspx) is available that suits my needs very well. Since my data binding was changing depending on other selections in the form, I was performing the binding in code, so my solution is also implemented in code. It is worth noting, however, that a CompositeCollection can also be created in Xaml using bindings.

I found a [nice guide](http://msdn.microsoft.com/en-us/library/ms742405.aspx) on MSDN that gave me the basic syntax. I added a CollectionContainer for my bound data and another CollectionContainer for my Add New item.

```
CompositeCollection collection = new CompositeCollection();

CollectionContainer dataContainer = new CollectionContainer();
dataContainer.Collection = myObject.myDataCollection;

CollectionContainer addNewItemContainer = new CollectionContainer();
dataContainer.Collection = new ItemType[] { addNewItemPlaceholder };

collection.Add(dataContainer);
collection.Add(addNewItemContainer);

ListBox1.ItemsSource = collection;
```

Now my original data collection remains accurate, and I have the added benefit of not having to add special logic to keep the Add New item at the end of the ListBox. Another great benefit is the CollectionContainer maintains grouping of collections, so I can add new items to the data collection and they automatically show up in the ListBox right above the Add New item.

