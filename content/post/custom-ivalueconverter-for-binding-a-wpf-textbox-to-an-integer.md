---
date: "2012-05-23T02:27:01Z"
description: ""
draft: false
slug: custom-ivalueconverter-for-binding-a-wpf-textbox-to-an-integer
title: Use A Custom IValueConverter To Bind A WPF TextBox To An Integer
---


In a WPF application, I had a `TextBox` bound to an integer property of a class. The `TextBox` only allows numeric input. The binding worked as expected except when the text was cleared. I found that when I cleared the text on my WPF `TextBox`, the binding would fail because it could not convert an empty string to an integer. This caused the previous value of the integer to remain set. If the application then took action on the integer property, it wasn’t apparent that it didn’t correctly reflect the user input.

I decided to get around this behavior by implementing my own custom `IValueConverter` to handle the empty string value and update the binding with a relevant value of my choosing. The `IValueConverter` interface has two methods, `Convert()` and `ConvertBack()`. In this case, the `Convert()` method is going to convert the incoming binding value (`integer`) to the `TextBox` binding value (`string`). The opposite will happen in the `ConvertBack()` method. The magic happens when the `integer` value to convert equals the `EmptyStringValue`, or when the `string` value to convert is empty.

```
public int EmptyStringValue { get; set; }

public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
{
    if (value == null)
        return null;
    else if (value is string)
        return value;
    else if (value is int && (int)value == EmptyStringValue)
        return string.Empty;
    else
        return value.ToString();
}

public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
{
    if (value is string)
    {
        string s = (string)value;
        if (s.IsNumeric(NumericValidator.Int32))
            return System.Convert.ToInt32(s);
        else
            return EmptyStringValue;
    }
    return value;
}
```

To use this custom converter, you can set it up as a resource in your WPF control, optionally setting the `EmptyStringValue`.

```
<Grid.Resources>
    <my:IntToStringConverter x:Key="customIntToStringConverter" EmptyStringValue="0" />
</Grid.Resources>
```

Then use it in the binding on the TextBox.

```
<TextBox Text="{Binding Path=IntegerBindingValuePath, Converter={StaticResource customIntToStringConverter}}" />
```

Now when the user clears the `TextBox`, the binding value is set to `0` while the TextBox displays an empty string. Once more text is entered, the binding value is updated to the correct value. This also has the effect of changing the input of `0` to an empty string, which may or may not be proper in a particular case. Hopefully this example can at least be a starting point for solving such problems.

###### Disclaimer

Use of this code is subject to this site’s [Terms of Use](__GHOST_URL__/terms)

