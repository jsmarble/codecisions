+++
date = 2014-01-14T14:35:00Z
description = ""
draft = false
slug = "the-context-pattern"
title = "The Context Pattern"

+++


I have found myself enjoying a certain coding pattern lately, so I thought it would be a good opportunity for a new post.

There are situations where you want to do something within a certain state, and ensure the state gets reverted at the completion of the operation. For example, while setting some properties, you want the `INotifyPropertyChanged` event to be temporarily suppressed, but ensure that it is functioning normally once these properties have been set.

To handle this, I’ve been writing `IDisposable` classes that encapsulate the state logic and ensure the state gets reverted when `Dispose()` is called. I know this is not something I invented, but I wanted to give it a name and give a quick example for anyone who is not familiar with the concept. I have named this pattern the Context Pattern, from the idea that a certain block of code is executing within the *context* of the `IDisposable` class.

Let’s look at an example based on the situation described above.

The class Foo implements `INotifyPropertyChanged`.

```
public class Foo : INotifyPropertyChanged
{
    private bool firePropertyChangedEvent = true;
    
    public void DisablePropertyChangedEvent()
    {
        this.firePropertyChangedEvent = false;  
    }
    
    public void EnablePropertyChangedEvent()
    {
        this.firePropertyChangedEvent = true;   
    }   
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (firePropertyChangedEvent)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, e);
        }
    }
    
    private string bar;
    public string Bar
    {
        get { return bar; }
        set 
        {
            if (bar != value)
            {
                bar = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bar")); 
            }
        }
    }
    
    public void DoWork()
    {
        this.Bar = "doing work";
    }
}
```

In the `DoWork()` method, you decide you do not want to fire the `PropertyChanged` event. You could call the `[Disable/Enable]PropertyChangedEvent` methods on Foo manually.

```
public void DoWork()
{
    this.DisablePropertyChangedEvent();
    this.Bar = "doing work";
    this.EnablePropertyChangedEvent();
}
```

However, if something fails in the `DoWork()` method, the class will be stuck in the wrong state. A `try/finally` block would ensure that didn’t happen, but the code starts getting a bit more terse and isn’t reusable. Enter the Context Pattern.

The `DisablePropertyChangedEventContext` class can handle that work and make sure that the class is left in the correct state when disposed.

```
public class DisablePropertyChangedEventContext : IDisposable
{
    private Foo foo;
    
    public DisablePropertyChangedEventContext(Foo foo)
    {
        if (foo == null) throw new ArgumentNullException("foo");
        
        this.foo = foo;     
        foo.DisablePropertyChangedEvent();
    }
    
    public void Dispose()
    {
        foo.EnablePropertyChangedEvent();
    }
}
```

Lastly, modify the `DoWork()` method to use the context. Now, setting the `Bar` property will not trigger the `PropertyChanged` event.

```
public void DoWork()
{
    using (new DisablePropertyChangedEventContext(this))
        this.Bar = "doing work";
}
```

[Download this example in LINQPad](https://dl.dropboxusercontent.com/u/107783/codecisions/ContextPattern.zip)

Of course this pattern is not limited to changing an object state temporarily. Another good use would be to wrap a block of code in a performance context that runs a stopwatch to time how long the code takes to execute. I will leave that class as an exercise for the reader.

