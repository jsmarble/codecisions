---
date: "2008-10-14T15:36:00Z"
description: ""
draft: false
slug: linq-to-sql-error-the-entity-is-not-of-the-correct-type
title: 'LINQ to SQL Error: The entity is not of the correct type.'
---


I came across an error while working on a LINQ to SQL sample application. The error was “The entity is not of the correct type.” and I could not find much information online about it. The one blog post I found indicated that the problem was related to using a Database View for the data, but I was just accessing the table directly.

But there was one part of the article that caught my attention. The author was discussing the creation of the view and making sure you have a primary key assigned. This made me realize that I had not assigned a primary key to my test database nor the class’s attribute mapping. After I fixed those two oversights, the the problem was resolved.

If you are receiving “The entity is not of the correct type.”, make sure you’ve assigned primary keys correctly.

