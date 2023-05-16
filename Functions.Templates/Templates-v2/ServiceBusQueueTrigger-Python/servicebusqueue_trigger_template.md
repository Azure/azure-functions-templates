# Azure Functions: Service Bus Queue Trigger in Python

## Service Bus Queue Trigger

Use the Service Bus Queue trigger to respond to messages from a Service Bus queue. Starting with extension version 3.1.0, you can trigger on a session-enabled queue.

## Using the Template

Following is an example code snippet for Service Bus Queue Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel) (currently in Preview).

```python
import logging
import azure.functions as func

app = func.FunctionApp()

@app.function_name(name="ServiceBusQueueTrigger1")
@app.service_bus_queue_trigger(arg_name="msg", queue_name="myinputqueue", connection="<CONNECTION_SETTING>")
def test_function(msg: func.ServiceBusMessage):
    logging.info('Python ServiceBus queue trigger processed message: %s',
                 msg.get_body().decode('utf-8'))
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- Confirm that the parameters within the trigger reflect values that correspond with your storage account.
- The name of the file must be `function_app.py`.
  
Note that Service Bus output bindings are also supported in Azure Functions. To learn more, see [Azure Service Bus bindings for Azure Functions](https://aka.ms/azure-function-binding-service-bus)

## V2 Programming Model

The v2 programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

To learn more about using the Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.