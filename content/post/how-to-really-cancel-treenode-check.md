+++
date = 2010-03-16T12:00:00Z
description = ""
draft = false
slug = "how-to-really-cancel-treenode-check"
title = "How To Really Cancel TreeNode Check"

+++


The [TreeView](http://msdn.microsoft.com/en-us/library/system.windows.forms.treeview.aspx) control can support checkboxes for nodes by setting the [CheckBoxes](http://msdn.microsoft.com/en-us/library/system.windows.forms.treeview.checkboxes.aspx) property to true. In my custom TreeView with checkboxes, I need to “disable” some nodes and not allow them to be checked. At quick glance, the [OnBeforeCheck](http://msdn.microsoft.com/en-us/library/system.windows.forms.treeview.onbeforecheck.aspx) virtual method seems to be the correct place to cancel the checking using the [TreeViewCancelEventArgs](http://msdn.microsoft.com/en-us/library/system.windows.forms.treeviewcanceleventargs.aspx) [.Cancel](http://msdn.microsoft.com/en-us/library/system.componentmodel.canceleventargs.cancel.aspx) property. If you have come to the same conclusion then let me confirm that you are indeed correct that this is the place to cancel node checking.

The problem comes when you double-click these “disabled” nodes. Sure enough the method executes and cancels perfectly. But that second click causes quite the trouble. Notice that the node now shows an inconvenient little checkmark next to it. Now this seems strange considering you canceled the event, but there it is, staring at you, clearly showing the checkmark. I’ll save you some research time here and say that the node’s checked state is actually not checked, invalidating the control for repaint does not fix it, and the double-click event handling is of no use.

This is clearly a bug in my book because how could you ever justify a visual state that is not the true state? You can’t. So, how do we stop this buggy behavior? After trying a few things I ended up having my TreeView override WndProc and stopping the WM_LBUTTONDBLCLK message from ever reaching the control. Now clearly this blanket approach would not be appropriate if you relied on double-clicking elsewhere in your control. If so, you might have to selectively block the message depending on other factors. This also requires you to create your own class that inherits TreeView, but in my case I was already.

Sometimes the only way around these things is to use the lower-level features of the framework such as windows messages and Win32 API calls. The important thing to remember is to use them if you have to because our job is to make things work in the best way possible, but they still have to work.

```
protected override void WndProc(ref Message m)
{
    const int WM_LBUTTONDBLCLK = 0x203;
    if (m.Msg != WM_LBUTTONDBLCLK)
        base.WndProc(ref m);
}
```

