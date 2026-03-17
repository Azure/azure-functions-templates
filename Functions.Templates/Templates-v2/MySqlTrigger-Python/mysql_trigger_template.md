# Azure Functions: MySQL in Python

## MySQL Trigger

The Azure Database for MySQL Trigger uses the Azure MySQL Change Feed to listen for inserts and updates across partitions. The change feed publishes inserts and updates, not deletions.

## Using the Template

Following is an example code snippet for MySQL Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel).

```python
import logging
import azure.functions as func
import json

app = func.FunctionApp()

@app.function_name(name="MySQLTrigger1")
@app.mysql_trigger(arg_name="changes", table_name="<TABLE_NAME>", connection_string_setting="MySqlConnectionString",
 leases_table_name="leases")
def test_function(changes: str) -> None:
    if changes:
        logging.info("MySQL Changes: ")    
    json_changes = json.loads(changes)
    for change in json_changes:
        rowdata = func.MySqlRow(change["Item"]) 
        logging.info(rowdata.data)
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- Confirm that the parameters within the trigger reflect values that correspond with your storage account.
- The name of the file must be `function_app.py`.
  
Note that MySQL input and output bindings are also supported in Azure Functions. 

## V2 Programming Model

The v2 programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

To learn more about using the Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.