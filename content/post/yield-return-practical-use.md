+++
date = 2009-01-22T13:54:00Z
description = ""
draft = false
slug = "yield-return-practical-use"
title = "Yield Return Practical Use"

+++


In my previous post about the “yield return” syntax, I highlighted my displeasure with the fact that it doesn’t actually enumerate the yielded values until accessed. In my opinion, this is dangerous for developers who do not clearly understand it’s use. The issue is that at face value it appears to simply be a shortcut for returning a collection, avoiding the need to explicitly declare the collection object and add items.

 In my random use of the yield return syntax, while trying to be mindful of the implementation details of its use, I found a seemingly practical use of the syntax. Imagine loading a large file of delimited values and inserting them into a database. You could simply read all the records and then enumerate them and insert them into the database. The drawback to this method is that you must load the entire file before doing any database work, which could take a while if you are dealing with millions of records. Now this is where the details on the workings of yield return come into play. If you returned each record in the file using yield return, you are not actually reading the file until you perform an operation on the collection. And even when you do, you are only accessing the records as you use them (unless you use Count() or some other aggregation method that looks at each record).

 Let me explain what I just said. You read the file using yield return, then you enumerate the records. What really happens is as you enumerate the records, the file is being read, one record at a time. So, you actually read a record, insert the record in the database, read another record, insert that record in the database, etc. Now this might not prove to be any faster in the end, but it does let you immediately start working with data rather than wait for all the records to be loaded. So at a minimum it is helpful for debugging. Another benefit is if you application is multithreaded, you can give users immediate feedback, such as “Processed X Records” rather than needing to say “Loading file” forever and then processing the records.

 For clarification, I will list what seemingly happens in code and then what actually happens at runtime.

<u>How The Code Reads</u>

1. Read all records in file and load into a collection.
2. Enumerate loaded records, inserting each record into the database.

 Summary: Load records A,B,C. Then insert records A,B,C.

<u>How The Code Runs</u>

1. Create a special collection that does not actually read the file until enumerated.
2. Enumerate this special collection, reading each record as Enumerator.MoveNext() is called.
3. During each record enumeration, insert the values for that record into the database.

 Summary: Load record A, then insert record A. Load record B, then insert record B. Load record C, then insert record C.

 Since I love code examples, download the attached sample project and put a breakpoint on line 30. Now, watch when you step over the method call to the method using yield return. Notice how you don’t actually enter the method? Now watch what happens when the collection is enumerated. Every time you move to the next item in the collection, the yield return on line 30 gets hit.

 So, I can see that there can be value in this behavior, but it sure feels weird still. Have another use for yield return? Leave a comment and explain.

[Yield Return Example](http://static.codecisions.com/YieldReturnExample.7z)

