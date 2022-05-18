# SQL Output Binding - C<span>#</span>

The `SQL Output Binding` makes it easy to take an array of rows and upsert them into the user table (i.e. If a row doesn't already exist, it is added. If it does, it is updated).

## How it works

For a `SQL Output Binding` to work, you provide the existing table in the database to upsert rows into. For instance, you can set the table to `[dbo].[table1]` in the `commandText` attribute to upsert into this table.

For more information, see the official [docs](https://aka.ms/sqlbindingsoutput)