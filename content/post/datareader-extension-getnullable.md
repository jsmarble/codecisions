+++
date = 2016-03-10T21:41:27Z
description = ""
draft = false
slug = "datareader-extension-getnullable"
title = "DataReader Extension GetNullable"

+++


It has always been a minor pain that fields in a database can be null while the same value type in .Net cannot be. If you wanted to read a nullable DateTime field in the database into your object’s DateTime property, the only choice was to check it for null first then read the field if not null.

```
if (!dr.IsDBNull(3))
    obj.Date = dr.GetDateTime(3);
```

I like to use extension methods to prevent the need to lookup ordinals every time I read from the data reader. I figured why not go ahead and write nullable extension methods for value types as well.

```
obj.Date = dr.GetNullableDateTime("datefield").GetValueOrDefault();
```

This approach would work, but it requires an extension method for every value type which will be read from the database. Instead, it would be nice to only have to write the logic once and be able to reuse it for every value type.

To do this, I wrote an extension method using generics and a predicate to check for null then read the value if not null. Note that the column name is passed to this method but the predicate takes the ordinal of the column. This is simply to prevent an extra wasted ordinal lookup inside the predicate.

```
public static T? GetNullable<T>(this IDataReader source, string column, Func<int, T> predicate) where T : struct
{
    int index = source.GetOrdinal(column);
    if (source.IsDBNull(index))
        return null;
    else
        return predicate(index);
}
```

Now the usage of this method is just as clean as the previous approach, but it lends itself to every value type. Additionally, if the property being set was nullable already then you could leave off the GetValueOrDefault() method call.

```
obj.IntValue = dr.GetNullable("intvalue", dr.GetInt32).GetValueOrDefault();
obj.Date = dr.GetNullable("datefield", dr.GetDateTime).GetValueOrDefault();
```

I hope this is useful to you! If you have another idea or suggestion, leave a comment.

