# Azure Functions: Blob Trigger in Python

## Blob Trigger

The Blob storage trigger starts a function when a new or updated blob is detected. The blob contents are provided as input to the function. The Azure Blob storage trigger requires a general-purpose storage account. Storage V2 accounts with hierarchical namespaces are also supported.

## Using the Template

Following is an example code snippet for Blob Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel).

```python
import logging
import azure.functions as func

app = func.FunctionApp()

@app.function_name(name="BlobTrigger1")
@app.blob_trigger(arg_name="myblob", path="samples-workitems/{name}",
                  connection="BlobStorageConnection")
def test_function(myblob: func.InputStream):
   logging.info("Python blob trigger function processed blob \n"
                f"Name: {myblob.name}\n"
                f"Blob Size: {myblob.length} bytes")
```

This example uses SDK types to directly access the underlying BlobClient object provided by the Blob storage trigger:

```python
import logging
import azure.functions as func
import azurefunctions.extensions.bindings.blob as blob

app = func.FunctionApp(http_auth_level=func.AuthLevel.FUNCTION)

@app.blob_trigger(arg_name="client", path="samples-workitems/{name}",
                  connection="BlobStorageConnection")
def blob_trigger(client: blob.BlobClient):
    logging.info(
        f"Python blob trigger function processed blob \n"
        f"Properties: {client.get_blob_properties()}\n"
        f"Blob content head: {client.download_blob().read(size=1)}"
    )
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- Confirm that the parameters within the trigger reflect values that correspond with your storage account.
- The name of the file must be `function_app.py`.
- If you are using SDK-Type Bindings, make sure to include `azurefunctions-extensions-bindings-blob` in your `requirements.txt` file. Visit [SDK Binding Types for Blob](https://aka.ms/functions-sdk-blob-python) for more information on how to use SDK-Type Bindings in your app.
  
Note that Blob input and output bindings are also supported in Azure Functions. To learn more, see [Azure Blob storage bindings overview](https://aka.ms/azure-function-binding-storage-blob).

## Programming Model V2

The new programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks.

The improved programming model requires fewer files than the default model, and specifically eliminates the need for a configuration file (`function.json`). Instead, triggers and bindings are represented in the `function_app.py` file as decorators. Moreover, functions can be logically organized with support for multiple functions to be stored in the same file. Functions within the same function application can also be stored in different files, and be referenced as blueprints.

To learn more about using the new Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.

To learn more about the new programming model for Azure Functions in Python, see [Programming Models in Azure Functions](https://aka.ms/functions-programming-models).
