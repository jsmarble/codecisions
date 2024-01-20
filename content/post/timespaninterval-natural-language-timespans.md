+++
date = 2016-09-02T02:19:11Z
description = ""
draft = false
slug = "timespaninterval-natural-language-timespans"
title = "TimeSpanInterval - Natural Language TimeSpans"

+++


Have you ever wanted to give your users some predefined choices for a time span (minutes, hours, etc)? That’s a pretty natural thing to do using language, but not such a natural thing to do using the `TimeSpan` class in .Net. You could write your own methods to convert from those options into an actual `TimeSpan`, but you’d likely be using switch statements to tie together the idea of a unit of time to the `TimeSpan` class.

Instead, I wrote a class called `TimeSpanInterval` that does the work for you and directly links the natural language idea of a unit of time to the `TimeSpan` class. The class supports any interval you can define, as long as you can provide delegates for converting to and from the `TimeSpan` class. I have provided the common ones already as static members of the `TimeSpanInterval` class.

To go from a `TimeSpanInterval` to a `TimeSpan`, just call the `ToTimeSpan()` method and provide the value representing the number of (seconds, minutes, hours) for the `TimeSpan`.  
```
TimeSpan tenMinutes = TimeSpanInterval.Minutes.ToTimeSpan(10);
```

This might not seem all that useful until you put it into the perspective of a user interface. Imagine a `ComboBox` whose items are the `TimeSpanInterval` values and bound to a property called `SelectedInterval`. Also imagine a `TextBox` bound to a integer property called `TimeValue`. The user enters “12” in the `TextBox` and selects “Minutes” from the `ComboBox`.  
![timespaninterval-wpf](https://static.codecisions.com/timespaninterval-wpf.png)

Without any other information or code, you can get a `TimeSpan` containing the user’s input. No switch statements or conditional logic required.  

```
TimeSpan tspan = SelectedInterval.ToTimeSpan(TimeValue);
```

You can also reverse the `TimeSpanInterval` part of the equation to go from a `TimeSpan` to a `TimeSpanInterval`. This is done by calling `InferFromTimeSpan()`, which uses a “best guess” approach by attempting to find the best unit match for the value of the `TimeSpan`.  

```
TimeSpanInterval interval = TimeSpanInterval.InferFromTimeSpan(timespan);
```

The code is hosted on gist.github.com, so give it a try and let me know what you think.

<div class="oembed-gist"><script src="https://gist.github.com/jsmarble/65befee9563859642d1278aaba742238.js"></script><noscript>View the code on [Gist](https://gist.github.com/jsmarble/65befee9563859642d1278aaba742238).</noscript></div>

