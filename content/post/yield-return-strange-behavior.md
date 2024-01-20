+++
date = 2008-10-29T15:53:00Z
description = ""
draft = false
slug = "yield-return-strange-behavior"
title = "Yield Return Strange Behavior"

+++


I was working on generating some test data today and realize something about “yield return” that troubles me. It seems to call the iteration code multiple times. Examples are the easiest way to demonstrate this behavior, so check the console application below.

```
static void Main(string[] args)
{
    IEnumerable<string> guids = GetGuids(4);

    foreach (string guid in guids)
        Console.WriteLine(guid);

    foreach (string guid in guids)
        Console.WriteLine(guid);

    Console.ReadLine();
}

static IEnumerable<string> GetGuids(int count)
{
    for (int i = 0; i < count; i++)
    {
        yield return Guid.NewGuid().ToString();
    }
}```

What output would you expect? I would expect to see 4 unique guids duplicated twice. Instead I see 8 unique guids. Try this yourself and put a breakpoint in the GetGuids() method. It doesn’t get hit until you iterate the IEnumerable<string> and it gets hit both times you enumerate it.</string>

Now replace the yield return with the standard logic of adding the guids to a List<string> and return the List<string> (still returning as IEnumerable<string>). This time the behavior is what I expect, with the guid list being generated once and enumerated twice.</string></string></string>

So, my question is … why in the world does the framework treat yield return differently? I would expect it to just be a compiler convenience to keep me from having to explicitly declare a List<string> and add the items. I think the real danger here is using yield return inside a loop that is doing work to get it’s results. If the work only needs to be done once, you are redoing the work every time you iterate the IEnumerable<t>. This seems dangerous to me.</t></string>

I will avoid yield return until I see its benefit in a specific situation. If you can explain the reasoning behind this behavior, please leave a comment.

***Update/Author's Note:** This article was written before I understood how deferred execution worked or the value it provides. I now use yield return in some situations where its behavior is desired. Don't necessarily avoid the yield keyword, just understand its place.*

**Update:** If you want to use yield return as in the example above but want the results duplicated for the second iteration, simply call .ToList() or .ToArray() on the GetGuids() method call.

