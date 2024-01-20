+++
date = 2010-04-08T12:00:00Z
description = ""
draft = false
slug = "enumerating-idatareader-with-linq"
title = "Enumerating IDataReader With LINQ"

+++


Today a coworker was trying to read an [IDataReader](http://msdn.microsoft.com/en-us/library/system.data.idatareader.aspx) into an anonymous type for an internal tool. This got me thinking about how [LINQ](http://msdn.microsoft.com/en-us/library/bb308959.aspx) methods could be useful when working with a DataReader since it’s basically a collection of records that you enumerate. In thinking about this, I came up with the DataReaderEnumerable class.

The DataReaderEnumerable is a simple class that wraps an IDataReader and implements IEnumerable<idatareader>. [IEnumerable<t></t>](http://msdn.microsoft.com/en-us/library/9eekhta0.aspx) is a very simple interface, and the guts of the enumerating actually happen in the IEnumerator<t> that is returned from the [GetEnumerator()](http://msdn.microsoft.com/en-us/library/s793z9y2.aspx) method. I created a similar class called DataReaderEnumerator that moves to the next IDataReader record when [MoveNext()](http://msdn.microsoft.com/en-us/library/system.collections.ienumerator.movenext.aspx) is called. This allows all of the LINQ methods to be used on a DataReader. </t></idatareader>

I have included the code for the DataReaderEnumerable and DataReaderEnumerator classes below. No warranty is given to the accuracy or quality of this code. Use at your own risk. If it kills puppies I am not responsible.

<script src="https://gist.github.com/jsmarble/ff70d97998c7fd1afaba5a2508df949d.js"></script>

For the extension lovers, I also found it useful to create an extension method for IDataReader call AsEnumerable() that returned an instance of DataReaderEnumerable.

```
public static DataReaderEnumerable AsEnumerable(this IDataReader source)
{
    return new DataReaderEnumerable(source);
}
```

To use this functionality, call a LINQ method like Select on the DataReaderEnumerable.

```
public List<User> GetUsers()
{
    //Get the data reader
    List<User> users = dr.AsEnumerable().Select(ReadUser).ToList();
    return users;
}

private User ReadUser(IDataReader dr)
{
    User user = new User();
    user.Id = dr.GetInt32(0);
    user.Username = dr.GetString(1);
    return user;
}
```

I’ll admit this is kind of funky behavior, so notice that there are some catches.

First, there is only one DataReader, even though being IEnumerable<idatareader> lends itself to appearing to contain multiple IDataReader objects. This means that any expressions used in LINQ will always be dealing with the same DataReader. It will simply have had its current record advanced.</idatareader>

Second, IDataReader does not support moving backwards, so only one pass is possible. Future calls to GetEnumerator() will result in an InvalidOperationException (at least in my implementation).

Third, IDataReader methods cannot be called after the connection is closed, so make sure to force the collection to enumerate prior to disposing of the connection, likely using the `ToList()` extension.

Fourth, IDataReader is supposed to be disposed, and given the single possible enumeration limitation, I decided to dispose the IDataReader when the IEnumerator<t> is disposed. This further underlines the point that only one enumeration is possible. I also made the DataReaderEnumerable implement [IDisposable](http://msdn.microsoft.com/en-us/library/system.idisposable.aspx). I don’t know that it really matters, but I thought since it wrapped an IDisposable object, it should also implement it and go ahead and try to dispose the wrapper IDataReader.</t>

Please note that this implementation is no more limited than direct use of the DataReader. However, given these limitations, it becomes apparent that if it’s desired to work with the actual data of the records or cause more than one iteration, the data must first and foremost be stored in a data structure, likely using the Select() extension method.

