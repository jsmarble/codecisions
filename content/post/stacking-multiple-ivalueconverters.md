---
date: "2012-10-24T20:51:22Z"
description: ""
draft: false
slug: stacking-multiple-ivalueconverters
title: Stacking Multiple IValueConverters
---


## Problem

I have had situations before where I needed to convert a binding value in multiple ways, but there was no way to apply multiple converters to the binding.

One example of this is wanting to bind the visibility of a control to the inverse of a boolean binding value. Without multiple converters there are only two options.


## Less Than Ideal Solutions

1. You can bind to a dummy property that is just the inverse of the real property.
2. You can create a custom converter that combines inverting a boolean and converting it to visibility.

Both options are messy. Adding a dummy property is simple enough but not elegant. Creating a custom converter is just asking for trouble and a waste of existing converter knowledge.


## Ideal Solution

The best solution is to be able to apply a converter on the boolean to inverse it, then apply a secondary converter to convert that value to a visibility. Sadly, this functionality is not included in the .Net framework.

### StackingConverter

To fill this gap, I wrote a new converter that I called the StackingConverter. It has a property that is a collection of converters to use. To perform *Convert*, it applies the converters to the binding value, passing the output of each converter as the input for the next. To perform *ConvertBack*, it simply does the same operation in reverse order.

```
public class StackingConverter : DependencyObject, IValueConverter
{
    private ConverterCollection converters = new ConverterCollection();
    public ConverterCollection Converters
    {
        get { return converters; }
    }

    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        object result = value;
        if (Converters != null)
        {
            foreach (IValueConverter c in Converters)
            {
                result = c.Convert(result, targetType, parameter, culture);
            }
        }
        return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        object result = value;
        if (Converters != null)
        {
            foreach (IValueConverter c in Converters.Reverse()) //ConvertBack in reverse
            {
                result = c.Convert(result, targetType, parameter, culture);
            }
        }
        return result;
    }

    #endregion
}

public class ConverterCollection : ObservableCollection<IValueConverter> { }

```

### Usage

This converter can be used in Xaml like a normal converter, and the underlying converters can be specified in Xaml as well. Using the example scenario from the beginning of the article, this would be the solution using the StackingConverter.  
 In this Xaml, I use a custom converter, which simply inverts a boolean, along with the standard BooleanToVisibilityConverter.

```
<my:StackingConverter x:Key="invBoolToVis">
    <my:StackingConverter.Converters>
        <my:InverseBooleanConverter />
        <BooleanToVisibilityConverter />
    </my:StackingConverter.Converters>
</my:StackingConverter>
```

### Potential Problems

The main problem I see in this implementation is with the parameters passed to the converter. Since multiple conversions are being performed, I am unsure how meaningful the `targetType` parameter becomes.

Also, the converters obviously must be stacked in such as way that the output from a preceding converter makes sense as input to the following converter. This is no different than making sure any converter works with the binding value, though, so it is not much of a concern.

###### Disclaimer

Use of this code is subject to this site’s [Terms of Use](__GHOST_URL__/terms)

