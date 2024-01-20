+++
date = 2015-10-26T13:53:06Z
description = ""
draft = false
slug = "concurrent-operations-with-reactive-extensions"
title = "Concurrent Operations with Reactive Extensions"

+++


Today I encountered a scenario which I could describe with words much better than I could describe with code. I was reading results from a REST API, reading a few thousand records over multiple requests. I was saving these records to a database and didn’t want to wait until all records were pulled before saving, so I implemented some LINQ magic to enumerate the items in between each HTTP request.

The basic idea is this:  
 Request first batch -> Save batch -> Request next batch -> Save batch (repeat until no more records)

This can be achieved with LINQ through the magic of deferred execution.

```
IEnumerable<IList<Record>> recordBatches = GetBatches();
IEnumerable<Record> records = recordBatches.SelectMany(x => x);
records.Select(x => Save(x)).ToList();
```

The code was fairly straightforward and worked well, but I kept feeling like I should be making the next HTTP request at the same time that I was enumerating the previous results and saving to the database. Saying that I wanted to do this work in parallel was much easier than actually writing the code to do so.

My previous experience with trying to make multiple things happen concurrently was to use the Task framework with `async/await`. I started down this path but quickly realized I didn’t have a good way to put the results from each HTTP request into my enumerable. I tried to use deferred execution with `yield return` but you can’t do that from an `async` method. I then tried to use LINQ to aggregate all the task operation results, but I kept running into needing to wait on the task before I could return the results which defeated the whole purpose. While researching online to see if anyone had a solution to such a situation, I discovered a whole new way of thinking about collections and this kind of operation. I discovered the Reactive Extensions and `IObservable<T>`.

I will admit that I had heard of `IObservable<T>` before, but I never really knew what benefits it provided or why one would use it. It took me a few minutes of reading and thinking before I was able to mentally apply it to my situation, but once I got it figured out I realized it was exactly what I needed. It really turns on its head the way you are used to thinking about collections and enumerating data.

Instead of populating a list or defining an enumerable then returning it, `Observable<T>` allows you to return an object that will later be populated asynchronously. The `async` delegate given when creating the `Observable<T>` will push items into it as they are available. This means you can separate the loading of your data from the processing and even do it in parallel!

Concurrently:  
 1. Request first batch -> Request next batch (repeat until no more records)  
 2. Enumerate batches observable, saving each batch as it becomes available

```
public IObservable<IList<Record>> GetRecordBatches()
{
    return Observable.Create<List<Record>>(async obs =>
    {
        //Loop to read issues from the api 
        //then push them onto the observable
        while (!done)
        {
            var records = await QueryRecords();
            obs.OnNext(records);
        }
    });
}
```

When the consumer of the `Observable<T>` object tries to enumerate it, it will enumerate as far as it can, then block until the next item is available. The Reactive Extensions even have the ability to return an `IObservable<T>` as an `IEnumerable<T>` which can help integrate it into existing code without having to change many method signatures. The code from the first example could still work with the `IObservable<T>.ToEnumerable()`.

```
IEnumerable<IList<Record>> recordBatches = GetRecordBatches().ToEnumerable();
foreach (var batch in recordBatches)
{
    SaveBatch(batch);
}
```

Another option for consuming the `IObservable<T>` is to use the Subscribe method and provide a delegate to be used as a callback when an item is pushed to the observable.

```
IObservable<IList<Record>> recordBatches = GetRecordBatches();
recordBatches.Subscribe(batch => SaveBatch(batch));
```

It seems this would be the preferred method if you were not working within the confines of maintaining an existing usage of `IEnumerable<T>`.

While I don’t think this will drastically change how often I work with typical collections, having this tool in the coding tool belt could definitely prove useful in these kinds of situations.

*Edit: I meant to mention the Stack Overflow article that got me on the right path. http://stackoverflow.com/questions/18284169/how-to-yield-return-item-when-doing-task-whenany*

