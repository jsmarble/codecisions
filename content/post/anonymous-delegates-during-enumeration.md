+++
date = 2009-09-30T15:04:09Z
description = ""
draft = false
slug = "anonymous-delegates-during-enumeration"
title = "Anonymous Delegates During Enumeration"

+++


I had no idea what to title this post, but that’s the best I could come up with. I want to highlight an interesting scenario that can occur when trying to use anonymous delegates while enumerating a collection.

Consider the code below that creates buttons in a FlowLayoutPanel and adds a click event handler as an anonymous delegate. (For the purposes of the example, I’m not using the sender Button text, which would be a simple way to handle this scenario.)

```
List<string> numbers = new List<string>(new string[] { "One", "Two", "Three", "Four" });
foreach (string number in numbers)
{
    Button btn = new Button();
    btn.Text = number;
    btn.Click += new EventHandler((sender, e) => MessageBox.Show(number));
    flowLayoutPanel1.Controls.Add(btn);
}
```

Every time you click a Button you’ll see a MessageBox with the text “Four”. Now take the same code but use a seperate method to create the Buttons, still using an anonymous delegate for the click event handler.

```
List<string> numbers = new List<string>(new string[] { "One", "Two", "Three", "Four" });
foreach (string number in numbers)
{
    CreateButton(number);
}

private void CreateButton(string number)
{
    Button btn = new Button();
    btn.Text = number;
    btn.Click += new EventHandler((sender, e) => MessageBox.Show(number));
    flowLayoutPanel1.Controls.Add(btn);
}
```

Now when you click a `Button` you’ll see a `MessageBox` with the correct text.

I don’t pretend to understand how anonymous delegates are implemented, but I think I get the idea of why it works this way. The difference is in how anonymous delegates manage variable values. In the first example it simply points back to the number variable used in the loop which changes for each iteration. In the second example, the value of the parameter is stored off with the anonymous delegate and therefore is not changed upon the next iteration.

