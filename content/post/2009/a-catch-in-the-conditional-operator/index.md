---
date: "2009-11-02T14:00:00Z"
description: ""
draft: false
slug: a-catch-in-the-conditional-operator
title: A Catch in the Conditional Operator
---


Have you ever seen a [conditional (ternary) operator](http://msdn.microsoft.com/en-us/library/ty67wk28.aspx) seemingly evaluate the condition incorrectly?

Take a method that returns a display string using an object as input.

```
static string GetPersonSentence(Person p)
{
    return "The person's name is " + p != null ? p.Name : "Unknown";
}
```

Now consider calling this method with an instance of a person and with a null value.

```
GetPersonSentence(new Person { Name = "Bob" })
GetPersonSentence(null)
```

It seems like it should output fine both ways, but it breaks on the null value call. Can you tell why? Try putting parentheses around the conditional expression and run it again. It runs just fine. Now the problem may be more apparent, but it is still tricky. Have you found it yet?

In the broken code, it turns out that the operator is actually evaluating the quoted string along with the null check, so the evaluated value [“The person’s name is ” + p] is never null and also does not break because concatenating a null value to a string works fine. So the condition is true and it tries to use the p.Name property, resulting in the NullReferenceException. The parentheses told the operator exactly what to evaluate, separating the quoted string from the null check, and caused it to run as expected.

As with many things I find that seem to be not working correctly, the problem is the programmer rather than the code. The code does exactly what you told it to do. The problem is first you have to understand what you’re telling it.

