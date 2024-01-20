---
date: "2008-06-13T14:06:00Z"
description: ""
draft: false
slug: restricting-generics-to-be-not-so-generic
title: Restricting Generics To Be Not So Generic
---


Bear with me as this one might get long … Several times before I have run across a certain problem that I did not know how to handle. Allow me to describe the problem, then I’ll describe the two possible solutions to this problem and the inherent problems with each solution. Of course, what good would this post be without at least my final solution to these problems.

Consider having two very similar classes with slightly different functionality. These classes probably share many aspects, but work a little differently in a few methods. It makes sense to have these two classes inherit a base class that contains the shared functionality, with each class overriding methods in which they need to work differently.  
 Also consider that these classes are persisted to some sort of storage, such as a database, and there are warehouse classes created for these objects. Now consider that these two warehouses also share much of the same functionality, if not exactly the same functionality as it relates to persisting the properties of the objects. So it makes sense to have one single warehouse that contains the persisting functionality. Therefore you are presented with two possible solutions.

Solution 1: Create a base warehouse which works with the objects using their base type.

Solution 2: Create a generic base warehouse which works with objects using the passed T.

Now, let me explain the problems with each.

Problem with Solution 1: A consumer of the warehouse does not want to get back the base object, they want to get back the specific type. Nobody wants to work with the abstract class all the time. Now you might think of creating a type-specific warehouse which inherits the base warehouse and does the casting, but then you realize that overriding the methods means you must keep the same signature, so you’re kind of stuck in the same place unless you want to create duplicate “wrapper” methods to get from the base class and then cast and return, but that’s kind of messy.

Problem with Solution 2: If using the passed type of T, you don’t know anything about T so it’s cumbersome to have to manually cast T to the base class each time you need to access a property. Plus, that kind of defeats the purpose of generics. However, this does somewhat avoid the problem of casting from the consumer’s point of view.  
 Really what we need is a way to implement solution 2 but still know that T is at least the type of the base object so we can work with it without always casting it around.

Well, thanks to the always helpful [Scott Hanselman](http://www.hanselman.com/blog) and his [post](http://www.hanselman.com/blog/LearningWPFWithBabySmashFactoriesInterfacesDelegatesAndLambdasOhMy.aspx) on improving [BabySmash](http://www.hanselman.com/babysmash/), I’ve learned a way to accomplish this. Scott wasn’t trying to post about this particular generics feature, but I picked it up while reading his code examples. You might have already known this, but I think it’s a downright awesome solution and I can’t believe I didn’t know this already.  
 You can specify that generic classes be a certain type. If that feels a little weird to you at first, it’s ok; it does to me, too. After all, aren’t generics supposed to be generic? But then again how many times do you want a generic class that actual works with the objects? Fairly often I’d say, and the only way to safely work with them is to at least know a little bit about them.  
 Below is the basic code snippet for how to accomplish this. Note that this is far too easy and makes you feel inept for not already knowing this.  

```
public class BaseObjectWarehouse<T> where T : BaseObject
{
    public T FindByID(int id)
    {
        // You can work with the properties and methods of T as BaseObject
        // but it will still be returned as the passed in T.
        // This also allows you to work with other generics easily.
        T value = Activator.CreateInstance<T>();
    }
}
```

So, am I crazy for not knowing this? Is it old hat to you? I think it's one of the greatest .Net things I've learned in a while. Know of a better way? If so, I'd love to hear it.

