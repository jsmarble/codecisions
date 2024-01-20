+++
date = 2008-08-19T16:11:13Z
description = ""
draft = false
slug = "another-reason-to-love-c-sharp"
title = "Another Reason to Love C#"

+++


It’s been a while since I’ve had a programming post, so here’s a quick one. Just to show the flexibility of C#, check out the code for the Control.CanSelectCore() method in C#.

```
internal virtual bool CanSelectCore()
{
    if ((this.controlStyle & ControlStyles.Selectable) != ControlStyles.Selectable)
    {
        return false;
    }
    for (Control control = this; control != null; control = control.parent)
    {
        if (!control.Enabled || !control.Visible)
        {
            return false;
        }
    }
    return true;
}
```

Notice how that FOR loop is used. I didn’t realize you could do that, but that makes sense. It’s basically a clean way of writing a DO-WHILE loop. Set ‘control’ to this object. Keep looping while ‘control’ is not equal to null. In each iteration, set ‘control’ to ‘control.Parent’. Think the VB FOR operator can handle this syntax? Nope, check the code for the same method in VB.

```
Friend Overridable Function CanSelectCore() As Boolean
    If ((Me.controlStyle And ControlStyles.Selectable) <> ControlStyles.Selectable) Then
        Return False
    End If
    Dim control As Control = Me
    Do While (Not control Is Nothing)
        If (Not control.Enabled OrElse Not control.Visible) Then
            Return False
        End If
        control = control.parent
    Loop
    Return True
End Function
```

It’s not that bad, but VB is really wordy. I love C# for so many other reasons, but this is just nice.

