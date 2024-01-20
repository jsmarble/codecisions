---
date: "2013-04-09T14:57:34Z"
description: ""
draft: false
slug: wpf-binding-boolean-to-an-enum-value
title: WPF Binding Boolean to an Enum Value
---


Imagine the situation where you have three radio buttons in a Window. You want the selection of those radio buttons to be bound to a single `enum` value where the `enum` contains values for each of the radio button options.

```
Radio Button: None
Radio Button: Blacklist
Radio Button: Whitelist

public enum FilterType
{
    None,
    Whitelist,
    Blacklist
}
```

When the bound value changes, the radio button selections should match it. When a radio button selection is changed, the bound value should match the selection.

*Problem*: How can you do this with only a binding?

*Solution*: Implement a custom `IValueConverter` to use on the binding.

```
public class EnumToBoolConverter : IValueConverter
{
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object trueValue, System.Globalization.CultureInfo culture)
    {
        if (value != null && value.GetType().IsEnum)
            return (Enum.Equals(value, trueValue));
        else
            return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object trueValue, System.Globalization.CultureInfo culture)
    {
        if (value is bool && (bool)value)
            return trueValue;
        else
            return DependencyProperty.UnsetValue;
    }

    #endregion
}
```

The idea of this converter is that you bind the converter parameter to the static value of the `enum` value for which it should return `true`. Any other value will return `false`. The reverse conversion works the same way, returning the converter parameter if the binding is `true`, otherwise returning `DependencyProperty.UnsetValue`.

```
<Window.Resources>
    <my:EnumToBoolConverter x:Key="filterTypeConverter" />
</Window.Resources>
...
<RadioButton Name="rdoNone" Content="None" 
             IsChecked="{Binding FilterType, Converter={StaticResource filterTypeConverter}, ConverterParameter={x:Static my:FilterType.None}}" />
<RadioButton Name="rdoBlacklist" Content="Blacklist" 
             IsChecked="{Binding FilterType, Converter={StaticResource filterTypeConverter}, ConverterParameter={x:Static my:FilterType.Blacklist}}" />
<RadioButton Name="rdoWhitelist" Content="Whitelist" 
             IsChecked="{Binding FilterType, Converter={StaticResource filterTypeConverter}, ConverterParameter={x:Static my:FilterType.Whitelist}}" />
```

I hope this helps you think in a binding oriented way. WPF bindings are very powerful and make your life easier by reducing code and being declarative of the intent.

