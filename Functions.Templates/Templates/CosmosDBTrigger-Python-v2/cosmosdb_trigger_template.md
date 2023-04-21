# Azure Functions: Cosmos DB in Python

## Cosmos DB Trigger

The Azure Cosmos DB Trigger uses the Azure Cosmos DB Change Feed to listen for inserts and updates across partitions. The change feed publishes inserts and updates, not deletions.

## Using the Template

Following is an example code snippet for Cosmos DB Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel) (currently in Preview).

```python
import logging
import azure.functions as func

app = func.FunctionApp()

@app.function_name(name="CosmosDBTrigger1")
@app.cosmos_db_trigger(arg_name="documents", database_name="<DB_NAME>", collection_name="<COLLECTION_NAME>", connection_string_setting="AzureWebJobsStorage",
 lease_collection_name="leases", create_lease_collection_if_not_exists="true")
def test_function(documents: func.DocumentList) -> str:
    if documents:
        logging.info('Document id: %s', documents[0]['id'])
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- Confirm that the parameters within the trigger reflect values that correspond with your storage account.
- The name of the file must be `function_app.py`.
  
Note that Cosmos DB input and output bindings are also supported in Azure Functions. To learn more, see [Azure Cosmos DB storage trigger and bindings overview](https://aka.ms/azure-function-binding-cosmosdb-v2)

## V2 Programming Model

The v2 programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

To learn more about using the Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.