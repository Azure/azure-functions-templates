# SQL Trigger Binding - JavaScript

The `SQL Trigger Binding` monitors the user table for changes (i.e., row inserts, updates, and deletes) and invokes the function with updated rows.

## How it works

`SQL Trigger Binding` utilizes SQL change tracking functionality to monitor the user table for changes. As such, it is necessary to enable change tracking on the SQL database and the SQL table before using the trigger support.

For more information, see the official [docs](https://aka.ms/sqltrigger).