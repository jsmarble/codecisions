+++
date = 2009-09-23T13:27:00Z
description = ""
draft = false
slug = "extensions-for-ienumerable-of-custom-classes"
title = "Extensions For IEnumerable<T> Of Custom Classes"

+++


Imagine a scenario in which you have a class called ‘Order’ and each order has a collection of ‘LineItem’. You have a collection of Order objects and want to get all of the LineItem objects for the Order collection. You could easily do this using lambda expressions and Linq. But if this functionality is commonly needed, you might find yourself repeating your lambda expressions in several places.

The real problem is that you don’t have a good place to encapsulate that logic because you are always dealing with the IEnumerable<order> rather than the order class. So, where can you put this code and share it in your application?</order>

One solution would be to add a method to your Order collection called ‘AggregateLineItems’, this way anywhere you have an IEnumerable<order> you can easily get all of the line items. Using extensions methods, you can do just that. Extensions methods can be coded to a specific IEnumerable<t> implementation and will therefore only apply to collections of that type.</t></order>

```
public static IEnumerable<LineItem> AggregateLineItems(this IEnumerable<Order> source)
{
    List<LineItem> lineItems = new List<LineItem>();
    foreach (Order order in source)
        lineItems.AddRange(order.LineItems);
    return lineItems;
}
```

Now you can call ‘AggregateLineItems’ from anywhere that knows about your extension method.

```
IEnumerable<Order> customerOrders = customer.Orders;
IEnumerable<LineItem> lineItems = customerOrders.AggregateLineItems();
```

