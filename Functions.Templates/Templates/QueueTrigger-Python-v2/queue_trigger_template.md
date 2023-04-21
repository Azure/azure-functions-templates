# Azure Functions: Queue Trigger in Python

## Queue Trigger

The queue storage trigger runs a function as messages are added to Azure Queue storage.

## Using the Template

Following is an example code snippet for Queue Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel) (currently in Preview).

```python
import logging
import azure.functions as func

app = func.FunctionApp()

@app.function_name(name="QueueTrigger1")
@app.queue_trigger(arg_name="msg", queue_name="python-queue-items",
                   connection=""AzureWebJobsStorage"")  
def test_function(msg: func.QueueMessage):
    logging.info('Python EventHub trigger processed an event: %s',
                 msg.get_body().decode('utf-8'))
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- Confirm that the parameters within the trigger reflect values that correspond with your storage account.
- The name of the file must be `function_app.py`.
  
Note that Queue output bindings are also supported in Azure Functions. To learn more, see [Azure Queue storage trigger and bindings for Azure Functions overview](https://aka.ms/azure-function-binding-queue)

## V2 Programming Model

The v2 programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

To learn more about using the Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.