---
date: "2009-08-04T19:42:00Z"
description: ""
draft: false
slug: to-delete-or-not-to-delete
title: To Delete Or Not To Delete
---


One of the decisions I regularly face when dealing with database data is whether to delete rows or mark them as deleted. On the one hand, the organized side of me really despises the idea of having old “deleted” records hanging around clogging up my queries. But, on the other hand, if you have the record there and for some reason determine that you need it back, it’s so nice to be able to just flip a flag and undo the delete.

I definitely see that marking as deleted or inactive is the only solution when you need to keep the record as a foreign key to another record while still removing it as a valid choice to the end user. But in the cases where the data truly is not used any longer, should you delete it?

Personally, I tend to not make decisions based on what-if scenarios and would rather code to the best practices of the 99% scenario. In some ways I’d like to delete data and rely on a good backup strategy for the rare cases when you need to restore records. But given the cheap cost of storage and minimal overhead of keeping old records, I can see why it may be best to keep the data and just ignore it from queries.

