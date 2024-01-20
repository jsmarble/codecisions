+++
date = 2009-02-12T19:58:00Z
description = ""
draft = false
slug = "learning-lambdas-tips-and-tricks"
title = "Learning Lambdas - Tips and Tricks"

+++


Anyone who spends much time looking at lambda expressions quickly realizes that they rock and are very clean and useful. I find myself using simple ones every day, but I thought I’d lay out some quick tips for anyone that shares my fondness of lambdas. If you’ve never learned about lambdas, hopefully this will help you get started.


## Introduction

Lambda expressions are nothing more than a shorthand syntax for delegates. Anything that passes a Func<>, Predicate<>, Action<>, or any other delegate can use a lambda for a clean implementation.


## A Simple Example

```
List<string> names = new List<string>();
names.AddRange(new string[] { "John", "Mark", "Jacob", "Greg", "Adam" });
var namesThatStartWithJ = names.Where(x => x.StartsWith("J"));```

Notice the “x =>” part? That just defines the parameter that you would normally pass to the delegate. A more verbose implementation would be as follows.

```
List<string> names = new List<string>();
names.AddRange(new string[] { "John", "Mark", "Jacob", "Greg", "Adam" });
var namesThatStartWithJ = names.Where(new Func<string, bool>(delegate(string x)
{
    return x.StartsWith("J");
}));```

Also notice In the first example, you did not have to specify that you were returning a boolean or even what value you were returning.


## Multiple Parameters

You will quickly find that there are cases in which you need to take more than one parameter in a lambda. In that case, simple enclose the parameters in parentheses before the “=>”.

`Comparison<string> comparison = new Comparison<string>((x, y) => x.CompareTo(y));`

Now isn’t that so much cleaner than specifying a full anonymous delegate?


## Multiple Statements

Some times, you have the need to specify multiple statements in a lambda. To do so, enclose the statements in {}. For example, let’s say you want to iterate all items in a List<datetime> and conditionally take actions.</datetime>

```
List<DateTime> birthdates = GetBirthdates();
birthdates.ForEach(x =>
{
    TimeSpan age = DateTime.Today.Subtract(x);
    Console.WriteLine(age.TotalDays);
});```


## Type Inference

Another option for the previous example would be to convert each birthdate to a TimeSpan first, then write the TimeSpan values out.

```
List<DateTime> birthdates = GetBirthdates();
IEnumerable<TimeSpan> agesInDays = birthdates.ConvertAll(x => DateTime.Today.Subtract(x));
agesInDays.ToList().ForEach(x => Console.WriteLine(x.TotalDays));```

It’s important to note the incredible fact that lambda expressions use type inference to determine what return type is used. I didn’t need to specify that I was converting the objects to TimeSpan. The fact that my lambda called DateTime.Today.Subtract(x) indicates that I’m converting to TimeSpan.

Also, you may have noticed that I converted my IEnumerable<timespan> to a List<> in order to call ForEach. I don’t understand why an enumeration method was not included with the other LINQ extensions in the 3.5 framework. Seeing as how it simply enumerates the list, it doesn’t seem List<> specific to me. I tend to create my own ForEach extension in my projects as it comes in handy often. For more information on Extension methods, see my previous post.</timespan>


## Assigning Lambdas To Delegates

Looking back at that example using the Comparison<string>, we can actually trim that up a little.</string>

`Comparison<string> comparison = (x, y) => x.CompareTo(y);`

You don’t even have to specify a “new Comparison<string>()”. You can assign delegates directly to a lambda expression and the compiler just figures it all out.</string>

