+++
date = 2008-09-23T13:56:00Z
description = ""
draft = false
slug = "binding-multiple-types-to-a-datagridview"
title = "Binding Multiple Types To A DataGridView"

+++


I’ve been having a problem lately with DataGridView binding. I have an interface (IAllergen) which is implemented by three different classes (Drug, DrugIngredient, and AllergenGroup). I want to bind a BindingList<object> to a DataGridView that has a column for one of the properties of IAllergen.

My problem with this binding situation is actually two fold. Initially, the implementations of IAllergen were explicit, meaning the interface members only are accessible when explicitly cast as IAllergen. This caused the DataGridView to not think that the object had the specified property, even though the list is typed as List<IAllergen> and the property clearly exists. This makes me think that the DataGridView is looking at the objects as their true type, which makes sense on some levels, so that’s fine.

My next attempt was to make the IAllergen implementations implicit by just providing the appropriate properties on each class. This got past the initial issue, but now during display of a row I get a ‘TargetInvocationException’ with the message “Object does not match target type.” My guess here is that the DataGridView looks at the first object in the bound collection and wants every subsequent item to be of the same type. Is it that difficult for the DataGridView to work for implementations of an interface?

All along I’ve been working with a BindingList<object> due to the fact that this logic is contained in a custom user control that is generic across multiple types of data and there is no good way to make a control use generics (without losing design time support). Using the immediate window in Visual Studio, I was able to force the data source to be a List<IAllergen> and that solved the problem. I suppose that the DataGridView tries to determine the object type based on the generic list type, and if it can’t determine it then it uses reflection to find the type, but expects all objects to be of the same type.

Whew, at least I’ve got a starting place. Now I just need to figure out how to abstract a generic controller class that can use strongly typed collections and return the results back to the UI in an untyped fashion for display. Welcome MVC design practices.

