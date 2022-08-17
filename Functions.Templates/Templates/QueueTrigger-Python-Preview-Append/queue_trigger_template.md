# Azure Functions: Queue Trigger in Python

## Queue Trigger

The queue storage trigger runs a function as messages are added to Azure Queue storage.

## Using the Template

Following is an example code snippet for Queue Trigger using the [new programming model](https://aka.ms/pythonprogrammingmodel) (currently in Preview).

```python
import logging

import azure.functions as func

app = func.FunctionApp()

@app.function_name(name="QueueTrigger1")
@app.queue_trigger(arg_name="msg", queue_name="python-queue-items",
                   connection="")  
def test_function(msg: func.QueueMessage):
    logging.info('Python EventHub trigger processed an event: %s',
                 msg.get_body().decode('utf-8'))
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- Confirm that the parameters within the trigger reflect values that correspond with your storage account.
- The name of the file must be `function_app.py`.
  
Note that Queue output bindings are also supported in Azure Functions. To learn more, see [Azure Queue storage trigger and bindings for Azure Functions overview](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue?tabs=in-process%2Cextensionv5%2Cextensionv3&pivots=programming-language-python)

## New Programming Model (Preview)

The new programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

The improved programming model requires fewer files than the default model, and specifically eliminates the need for a configuration file (`function.json`). Instead, triggers and bindings are represented in the `function_app.py` file as decorators. Moreover, functions can be logically organized with support for multiple functions to be stored in the same file. Functions within the same function application can also be stored in different files, and be referenced as blueprints.

To learn more about using the new Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://docs.microsoft.com/azure/azure-functions/functions-reference-python?tabs=asgi%2Capplication-level). Note that in addition to the documentation, [hints](https://github.com/Azure/azure-functions-python-library/blob/dev/docs/ProgModelSpec.pyi) are available in code editors that support type checking with PYI files.

To learn more about the new programming model for Azure Functions in Python, see [Programming Models in Azure Functions](https://aka.ms/functions-programming-models).