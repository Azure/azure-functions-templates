# Azure Functions: Blob Trigger in Python

## Blob Trigger

The Blob storage trigger starts a function when a new or updated blob is detected. The blob contents are provided as input to the function. The Azure Blob storage trigger requires a general-purpose storage account. Storage V2 accounts with hierarchical namespaces are also supported.

## Using the Template

Following is an example code snippet for Blob Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel) (currently in Preview).

```python
import logging

import azure.functions as func

app = func.FunctionApp()

@app.function_name(name="BlobTrigger1")
@app.blob_trigger(arg_name="myblob", path="samples-workitems/{name}",
                  connection="")
def test_function(myblob: func.InputStream):
   logging.info(f"Python blob trigger function processed blob \n"
                f"Name: {myblob.name}\n"
                f"Blob Size: {myblob.length} bytes")
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- Confirm that the parameters within the trigger reflect values that correspond with your storage account.
- The name of the file must be `function_app.py`.
  
Note that Blob input and output bindings are also supported in Azure Functions. To learn more, see [Azure Blob storage bindings overview](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-blob?tabs=in-process%2Cextensionv5%2Cextensionv3&pivots=programming-language-python)

## Programming Model V2 (Preview)

The new programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

The improved programming model requires fewer files than the default model, and specifically eliminates the need for a configuration file (`function.json`). Instead, triggers and bindings are represented in the `function_app.py` file as decorators. Moreover, functions can be logically organized with support for multiple functions to be stored in the same file. Functions within the same function application can also be stored in different files, and be referenced as blueprints.

To learn more about using the new Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.

To learn more about the new programming model for Azure Functions in Python, see [Programming Models in Azure Functions](https://aka.ms/functions-programming-models).