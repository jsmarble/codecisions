+++
date = 2009-08-27T14:53:27Z
description = ""
draft = false
slug = "action-try-extension-method"
title = "Action.Try() Extension Method"

+++


I read an article by John Teage on the Los Techies blog that gave me an idea. The article was about a method he used to wrap an action in a Try/Catch block for testing purposes. This seems like a handy method in some cases, but having a method means you have to be in that scope for the method to be accessible or it has to be a globally shared method. Enter extension methods.

```
public static bool Try(this Action source)
{
    try
    {
        source();
        return true;
    }
    catch (Exception)
    {
        return false;
    }
}
```

I created an extension method for Action called “Try” that simply returns a boolean of whether or not the Action invocation succeeded. The cool thing about this now is it’s available everywhere that this DLL is referenced and it also can work on any framework method by casting the method pointer to an Action. For example, say you wanted to call Dispose() on an IDisposable object but wanted to know if it succeeded without caring what the exception is (this is just an example and may not be a great idea). You could simply cast the Dispose method to an Action and call Try().

```
((Action)idisp.Dispose).Try();
```

You may want to do the same thing but pass a parameter to the method. Fortunately, this same principle is easily applied to Action<t> delegates, too.</t>

```
public static bool Try<T>(this Action<T> source, T obj)
{
    try
    {
        source(obj);
        return true;
    }
    catch (Exception)
    {
        return false;
    }
}
```

A good example of this in action (excuse the pun) is calling `Image.Save(path)`. Just cast the `Save` method to an `Action<string>` and call `Try()`, passing the path as the argument.</string>

```
((Action<string>)image.Save).Try(@"C:\myimage.bmp");
```

Now this same principle cannot apply to `Func` delegates because they return a result which prevents you from returning the success `boolean`. There are ways around this using `out` parameters, but that just takes the elegance out of it in my opinion.

If you are looking for a way to encapsulate the invocation of delegates with multiple retries in case of failure, keep an eye out for a future post where I’ll discuss a way to accomplish that using extension methods.

