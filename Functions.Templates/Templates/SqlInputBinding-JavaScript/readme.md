# SQL Input Binding - JavaScript

The `SQL Input Binding` makes it easy to retrieve data from a database, returning the output of the query or stored procedure to the function.

## How it works

For a `SQL Input Binding` to work, you can provide the query to retrive data from an existing object in the database. For instance, you can set the query to `Select * From [dbo].[table1]` to query `[dbo].[table1]` in the `commandText` value in your `function.json`.

For more information, see the official [docs](https://aka.ms/sqlbindingsinput).