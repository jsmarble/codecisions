+++
date = 2008-07-14T18:07:00Z
description = ""
draft = false
slug = "tempfilecollection"
title = "TempFileCollection"

+++


In [Scott Hanselman’s post](http://www.hanselman.com/blog/BackToBasicsEveryoneRememberWhereWeParkedThatMemory.aspx) on Garbage Collector and issues involved with playing sounds, I noticed a framework class, [TempFileCollection](http://msdn.microsoft.com/en-us/library/system.codedom.compiler.tempfilecollection.aspx), which I had never seen. He barely even mentioned it, not really giving much information about it, so I looked it up myself to see what it offers. You can read the [MSDN entry](http://msdn.microsoft.com/en-us/library/system.codedom.compiler.tempfilecollection.aspx) on the class if you want the details, but I’m going to offer some quick information that I learned when trying the class in a console app. The class does not offer anything you can’t handle yourself, but then again that’s the point of the .Net framework. One caveat that I found was when using the [AddExtension](http://msdn.microsoft.com/en-us/library/68yd12wb.aspx) method, passing in the file extension. When passing an extension of “tmp”, I got an exception saying the file had already been added to the collection, which is odd because I just created the collection. Upon closer inspection, I found that the first time the BasePath is accessed, it ensures that the temp path is set, and when doing so adds a temp file with the “tmp” extension. Since the BasePath property is accessed before any attempt to add a file to the collection, it is impossible to add a file with the “tmp” extension. However, if you try to access the pre-loaded item in the list, you’ll find two things.

1. Getting an item from the collection is not as simple as just giving it an index or a key. I’d like to actually be able to track my items using the collection and not have to keep track of them myself. I was able to get the first item using the code below, which leads to discovery two. `[TempFileCollection].OfType<string>().First();`
2. Even though the “tmp” extension is impossible to add yourself, it is not created until the BasePath property is accessed, which leaves me wondering why in the world they preload an item if it’s not easy to access and not necessarily available right away.

Another interesting thing to do is use [Reflector](http://www.aisto.com/roeder/dotnet/) to browse the source for the class. You’ll find two interesting things.

1. All path building is done by concatenating strings, not using [Path.Combine](http://msdn.microsoft.com/en-us/library/system.io.path.combine.aspx)(). Since the class allows for the base path to be provided in the constructor, I’d assume that the author would want to use a safe method for building the path since it is not known whether the provided path would have trailing backslashes. The lack of use of Path.Combine() here might lead me to believe that the author had some reason for not wanting to use the System.IO.Path methods, but if you dig into the code to actually ensure the base path is created, you’ll see interesting item two. The class gets a temp file name using it’s own custom method to handle the backslash problem then uses the [Path.GetFullPath](http://msdn.microsoft.com/en-us/library/system.io.path.getfullpath.aspx)() method to extract the path from this newly created temp file. Then, that newly created temp file is given the “tmp” extension and becomes our mystery preloaded “tmp” file. This all leaves me with asking a big WTF? Am I an idiot and don’t understand the logic for all of this? It sure seems like this could be done simpler and safer.
2. When disposed, the class tries to delete the files in the collection. If you look at the delete method, it’s just a [File.Delete](http://msdn.microsoft.com/en-us/library/system.io.file.delete.aspx)() wrapped in a try/catch with nothing else. I find it surprising that it’s not at least catching only an [IOException](http://msdn.microsoft.com/en-us/library/system.io.ioexception.aspx), if not having some basic retry logic. Oh well, I guess lingering temp files are not a big deal. And I can’t imagine any exception other than an IOException coming from that method, but if there is some other exception, I might want to know about it.

Despite those annoyances and questioning of the code, I find the TempFileCollection class still useful if you want to quick and dirty get some temp files and <span style="text-decoration: line-through;">trust</span> hope that they get deleted when you’re done with the collection. However, I’d like using it better if it was a little more friendly and I didn’t have the questions about its logic. I will give it one high mark in that the temp files are not created as soon as you add them to the collection. You actually have to write something to the file to create it. This seems like obvious logic, but I wouldn’t have been surprised if it created it immediately. See below for my code snippet used in testing and corresponding output.

```
static void Main(string[] args)
{
    string tmp = string.Empty;
    using (TempFileCollection files = new TempFileCollection())
    {
        tmp = files.AddExtension("sample");
        Console.WriteLine(string.Format("Created file : {0}", Path.GetFileName(tmp)));
        CheckFileExists(tmp);
        using (StreamWriter writer = new StreamWriter(tmp))
        {
            CheckFileExists(tmp);
            Console.WriteLine("Created StreamWriter");
            writer.Write(Guid.NewGuid().ToString());
            Console.WriteLine("Wrote GUID");
            CheckFileExists(tmp);
        }
        CheckFileExists(tmp);
    }
    CheckFileExists(tmp);
}

static void CheckFileExists(string path)
{
    Console.WriteLine(string.Format("File Exists .. {0}", File.Exists(path)));
}
```

Created file : c87jnzil.sample  
 File Exists .. False  
 File Exists .. True  
 Created StreamWriter  
 Wrote GUID  
 File Exists .. True  
 File Exists .. True  
 File Exists .. False

