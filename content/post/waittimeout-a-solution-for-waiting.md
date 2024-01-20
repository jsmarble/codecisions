---
date: "2015-01-16T20:29:25Z"
description: ""
draft: false
slug: waittimeout-a-solution-for-waiting
title: WaitTimeout - A Solution For Waiting
---


I recently found myself needing to periodically check the status of another operation, waiting for it to finish. In pseudocode, the basic operation is similar to this:

```
while (!complete)
{
    //check the status
    if ([completed])
        complete = true;
    else
        //sleep 500ms
}
```

I did not want to wait forever, though, so I wanted to implement a timeout. I could have used a timestamp, then checked the elapsed time with each iteration, but I wondered if there was a more reusable approach. I decided to write my own class to handle this admittedly simple situation.

```
public sealed class WaitTimeout : IDisposable
{
    private Stopwatch sw;
    private TimeSpan timeout;

    private WaitTimeout(TimeSpan timeout)
    {
        this.timeout = timeout;
        this.sw = new Stopwatch();
        sw.Start();
    }

    public static WaitTimeout Start(TimeSpan timeout)
    {
        WaitTimeout wt = new WaitTimeout(timeout);
        return wt;
    }

    public bool TimedOut
    {
        get
        {
            if (this.sw == null)
                throw new ObjectDisposedException(this.GetType().Name);
            return sw.Elapsed >= this.timeout;
        }
    }

    public void Dispose()
    {
        if (this.sw != null)
        {
            this.sw.Stop();
            this.sw = null;
        }
    }
}
```

Now I can quickly add a timeout check to my loop without worrying about timestamps and comparisons.

```
using (WaitTimeout wait = WaitTimeout.Start(TimeSpan.FromMinutes(2)))
{
    while (!complete && !wait.TimedOut)
    {
        //check the status
        if ([completed])
            complete = true;
        else
            //sleep 500ms
    }
}
```

Even though it doesn’t save a ton of code, I think it makes for a clean and reusable solution. If you see of any problems or know of other simple solutions in the .Net framework, leave a comment and let me know.

