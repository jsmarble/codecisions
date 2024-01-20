+++
date = 2017-08-03T00:12:03Z
description = ""
draft = false
slug = "how-to-build-custom-json-using-json-net"
title = "How to Build Custom JSON Using JSON.Net"

+++


The serializer from [JSON.Net](http://www.newtonsoft.com/json) is really good, and the default output from `JsonConvert.SerializeObject` will work for most situations, but sometimes you want fine control over the structure of your JSON. Fortunately, JSON.Net also includes an API for building out custom JSON. You will need the Nuget package [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) and add the namespace `Newtonsoft.Json.Linq`.

To start, create a new [JObject](http://www.newtonsoft.com/json/help/html/t_newtonsoft_json_linq_jobject.htm) that will represent the root of the JSON. Call the `Add` method to add properties to the JSON.

```
JObject json = new JObject();
json.Add("id", 1);
```

When you want to add a level to the JSON, you can simply add another JObject as a property.

```
json.Add("user", 
	new JObject
	{
		{"username", "john"},
		{"password", "secret"},
	});
```

To add an array to your JSON, use the [JArray](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Linq_JArray.htm) class.

```
json.Add("emailAddresses",
	new JArray
	{
		"john@email.com",
		"jdoe@corp.com"
	});
```

You can combine these two concepts and add JObjects to the JArray.

```
json.Add("contacts",
	new JArray
	{
		new JObject 
		{
			{"name", "Matthew"},
			{"phone", "8885552222"},
		},
		new JObject
		{
			{"name", "Kristie"},
			{"phone", "8885553333"},
		}
	});
```

When you're done, you can use this JObject as part of another JObject, or you can get the JSON by calling the `ToString` method.

```
json.ToString();
```

If you were to do this with the example code above, the end result would be:

```
{
  "id": 1,
  "user": {
    "username": "john",
    "password": "secret"
  },
  "emailAddresses": [
    "john@email.com",
    "jdoe@corp.com"
  ],
  "contacts": [
    {
      "name": "Matthew",
      "phone": "8885552222"
    },
    {
      "name": "Kristie",
      "phone": "8885553333"
    }
  ]
}
```

Enjoy building your own custom JSON!

