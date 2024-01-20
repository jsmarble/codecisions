+++
date = 2013-09-03T19:06:26Z
description = ""
draft = false
slug = "doing-work-in-dependency-property-setters"
title = "Doing Work In Dependency Property Setters"

+++


I was working on a WPF app recently where I was binding a dependency property to the selection of a ComboBox. I needed to pass this value down to a domain object, so I decided I would modify the bound property setter to also set a property on my domain object. This seemed a fairly straightforward approach, commonly used in normal properties.

To my surprise, my property setter never got called and my domain object never had the value set. I thought surely something must be wrong with my binding and the selection of the ComboBox wasn’t getting set. I was again surprised when debugging to find that the value of my dependency property was indeed the value selected from the ComboBox, despite the fact that my setter was never called.

It appears that the dependency property acts more like a binding hint than an actual value gateway. The binding was setting values directly on the dependency property and not going through my setter. This seems like a bit of a limitation to not be able to inject logic into dependency property getters and setters like normal properties, but there very well could be a logical explanation.

