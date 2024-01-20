---
date: "2015-07-09T19:31:41Z"
description: ""
draft: false
slug: disposing-of-interfaces
title: Disposing of Interfaces
---


Sometimes you run across situations where there is just no good solution. No matter which option you choose, there are downsides. I found one such situation today when trying to design an interface. The code below are simple examples I wrote for the purposes of this post.

```
public interface INotifier
{
    void Notify(string message);
}
```

I had written my initial implementation, which happened to use a resource that needed disposing.

```
public class FileNotifier : INotifier, IDisposable
{
    private FileStream stream;

    public FileNotifier(string path)
    {
        this.stream = File.OpenWrite(path);
    }

    public void Notify(string message)
    {
        //use the stream
    }

    public void Dispose()
    {
        if (stream != null)
        {
            stream.Dispose();
            stream = null;
        }
    }
}    
```

Of course I went to put this object in a using block only to realize that the interface through which I using this object was not disposable, so it would not compile.

```
//Will not compile because INotifier is not IDisposable
using (INotifier notifier = new FileNotifier(@"C:\notify.txt"))
{
    notifier.Notify("example");
}
```

I could add IDisposable to the interface so that any implementation could be disposed, with some implementations requiring an empty Dispose method implementation.

```
public interface INotifier : IDisposable
{
    void Notify(string message);
}
```

I really dislike the idea of having an interface inherit IDisposable because, by the nature of being an interface, it is impossible to know that the implementation needs disposing. Also consider that you may not have access to modify the interface.

Another option is casting the object to IDisposable. This prevents the interface from leaking an implementation detail, and also prevents empty implementations of a Dispose method in classes which have no resources to dispose. One downside is that the syntax requires you to expand the scope of your object outside the using block. This strategy is made possible by the fact that the using block allows you to pass in a null object.

```
INotifier notifier = new FileNotifier(@"C:\notify.txt");
using (notifier as IDisposable)
{
    notifier.Notify("example");
}
```

Naturally, this solution still requires the instantiator of the class to know that the implementation might be disposable, but it opens up the option of not disposing if you are confident that the implementations you use will not require disposing. If there is a chance that you will be using a disposable implementation, simply cast the object to `IDisposable` and put in inside a using block. Please note that the casting referred to in this context uses the `as` operator. Doing a direct cast will fail at runtime if the object is not `IDisposable`.

Really, I do not love either of these options. I prefer the casting solution, but it is still not ideal. I have not been able to come up with any other options though.

