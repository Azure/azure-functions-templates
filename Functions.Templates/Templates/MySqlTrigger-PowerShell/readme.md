# MySql Trigger Binding - PowerShell

The `MySql Trigger Binding` monitors the user table for changes (i.e., row inserts, and updates) and invokes the function with updated rows.

## How it works

`MySql Trigger Binding` utilizes the 'az_func_updated_at' column in the table to monitor the user table for changes.
