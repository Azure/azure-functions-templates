# Azure Functions: Service Bus Topic Trigger in Python

## Service Bus Topic Trigger

Use the Service Bus Queue trigger to respond to messages from a Service Bus topic. Starting with extension version 3.1.0, you can trigger on a session-enabled topic.

## Using the Template

Following is an example code snippet for Service Bus Topic Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel) (currently in Preview).

```python
import logging

import azure.functions as func

app = func.FunctionApp()

@app.function_name(name="ServiceBusTopicTrigger1")
@app.service_bus_topic_trigger(arg_name="message", topic_name="mytopic", connection="", subscription_name="testsub")
def test_function(message: func.ServiceBusMessage):
    message_body = message.get_body().decode("utf-8")

    logging.info("Python ServiceBus topic trigger processed message.")
    logging.info("Message Body: " + message_body)
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- Confirm that the parameters within the trigger reflect values that correspond with your storage account.
- The name of the file must be `function_app.py`.
  
Note that Service Bus output bindings are also supported in Azure Functions. To learn more, see [Azure Service Bus bindings for Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus?tabs=in-process%2Cextensionv5%2Cextensionv3&pivots=programming-language-python)

## Programming Model V2 (Preview)

The new programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

The improved programming model requires fewer files than the default model, and specifically eliminates the need for a configuration file (`function.json`). Instead, triggers and bindings are represented in the `function_app.py` file as decorators. Moreover, functions can be logically organized with support for multiple functions to be stored in the same file. Functions within the same function application can also be stored in different files, and be referenced as blueprints.

To learn more about using the new Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.

To learn more about the new programming model for Azure Functions in Python, see [Programming Models in Azure Functions](https://aka.ms/functions-programming-models).