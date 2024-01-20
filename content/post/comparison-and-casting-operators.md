+++
date = 2008-11-11T14:10:00Z
description = ""
draft = false
slug = "comparison-and-casting-operators"
title = "Comparison and Casting Operators"

+++


Today, I’m writing about [operators](http://msdn.microsoft.com/en-us/library/8edha89s(VS.80).aspx) in .Net because I think they are a very powerful but underutilized feature of the framework. I’ve worked with comparison operators ( == !=) in the past, but never really implemented them in a production project. However, the are also casting operators, which allow you to provide implicit or explicit conversion functionality between custom types.

Operators can be used to provide custom logic for comparison of objects. For example, if you have a Dog class and a Cat class (assume they don’t both inherit an Animal abstract class) that need to be compared, perhaps based on Age, you could write an operator that can handle the comparison and allow you to simply check if Dog > Cat.

```
public static bool operator >(Dog x, Cat y)
{
  return x.Age > y.Age;
}

public static bool operator <(Dog x, Cat y)
{
  return x.Age < y.Age;
}
```

One thing to notice with comparison operators is that you must define the opposite comparison. You cannot define the ‘>’ operator without the corresponding ‘this article for overloaded operators).

So I’ve covered the basics of comparison operators, but there is another kind of operator you can implement to provide casting between types. For casting operators, you can mark them as either implicit or explicit. For implicit operators, you can simply set Dog = Cat without explicitly casting it. Of course, implicit operators also allow explicit casting, but explicit operators require an explicit casting. I’ve included both examples below, but implicit would be sufficient for the functionality unless you require different logic for explicit vs implicit casting.

```
public static implicit operator Cat(Dog dog)
{
    return new Cat(dog.Name);
}
 
public static explicit operator Cat(Dog dog)
{
    return new Cat(dog.Name);
}
```

