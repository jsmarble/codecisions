+++
date = 2016-12-07T23:41:07Z
description = ""
draft = false
slug = "consolecolorcontext-give-some-color-to-your-console-applications"
title = "ConsoleColorContext - Give Some Color to Your Console Applications"

+++


I previously wrote about the [Context Pattern](__GHOST_URL__/the-context-pattern/) where I used an example of temporarily turning off an event. I was recently writing a console application and came up with another use of the Context Pattern that I thought I would share.

Often in a console application, the need arises to output some text in a different color, perhaps for displaying errors and warnings, or maybe just to give the user some extra visual feedback. If an application changes the console colors, it needs to be able to change them back to the original values. The user might have custom colors or there is some other reason that the console is already set to a different color. 

Normally this would involve keeping track of the original color and changing it back after the colorful output is written.

```
ConsoleColor originalColor = Console.ForegroundColor;
Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("Error: Bad things happened.");
Console.ForegroundColor = originalColor;
```

If this were done multiple times, it would result in some very redundant code. It would be possible to write a helper method that allowed some code sharing, but that would make the flow a bit more awkward.

```
public static void ConsoleWriteLineInColor(string message, ConsoleColor color)
{
    ConsoleColor originalColor = Console.ForegroundColor;
    Console.ForegroundColor = color;
    Console.WriteLine(message);
    Console.ForegroundColor = originalColor;
}
```
```
ConsoleWriteLineInColor("Error: Bad things happened.", ConsoleColor.Red);
ConsoleWriteLineInColor("additional error information", ConsoleColor.Red);
```

That is better than writing the same code over and over again, but it requires the message to be passed in as an argument and doesn't allow for multiple things to be written to the console without multiple calls to the helper method.

What would be really nice is a way to keep the same console writing method calls but simply apply a color to them all, having it automatically revert when finished. This is where the ConsoleColorContext comes in.

```
using (ConsoleColorContext.Create().WithForeground(ConsoleColor.Red))
{
    Console.WriteLine("Error: Bad things happened.");
    Console.WriteLine("additional error information");
}
```

Now the console color can be changed once, applied to all of the console writes within the context block, then reverted automatically. It can easily be applied to existing console writes with no modifications to how they work because it's just wrapping them in a context. I also made the class fluent, so you can further specify the background color and even the colors to restore when the context completes.

One other thing to note is that if you have some complex or existing coloring logic, you can keep it in place and still use the ConsoleColorContext to revert to the original colors. Simply use ConsoleColorContext.Create() without using any additional fluent methods.

```
using (ConsoleColorContext.Create())
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Error: Bad things happened.");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("additional error information");
}
```

I hope this idea further illustrates the power of the Context Pattern.

`ConsoleColorContext` is now available as a [nuget package](https://www.nuget.org/packages/ConsoleColorContext/)!

The code is hosted on gist.github.com, so check it out and start giving your console applications some color!
<script src="https://gist.github.com/jsmarble/b12f8e9c8454588fec98d67688867f82.js"></script>

