# Azure Functions: Service Bus Topic Trigger in Python

## Service Bus Topic Trigger

Use the Service Bus Queue trigger to respond to messages from a Service Bus topic. Starting with extension version 3.1.0, you can trigger on a session-enabled topic.

## Using the Template

Following is an example code snippet for Service Bus Topic Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel).

```python
import logging
import azure.functions as func

app = func.FunctionApp()

@app.function_name(name="ServiceBusTopicTrigger1")
@app.service_bus_topic_trigger(arg_name="message", topic_name="mytopic", connection="<CONNECTION_SETTING>", subscription_name="testsub")
def test_function(message: func.ServiceBusMessage):
    message_body = message.get_body().decode("utf-8")
    logging.info("Python ServiceBus topic trigger processed message.")
    logging.info("Message Body: " + message_body)
```

This example uses SDK types to directly access the underlying ServiceBusReceivedMessage object provided by the Service Bus trigger (currently in Preview):

```python
import logging
import azure.functions as func
import azurefunctions.extensions.bindings.servicebus as servicebus

app = func.FunctionApp(http_auth_level=func.AuthLevel.FUNCTION)

@app.service_bus_topic_trigger(arg_name="receivedmessage",
                               topic_name="TOPIC_NAME",
                               connection="SERVICEBUS_CONNECTION",
                               subscription_name="SUBSCRIPTION_NAME")
def servicebus_topic_trigger(receivedmessage: servicebus.ServiceBusReceivedMessage):
    logging.info("Python ServiceBus topic trigger processed message.")
    logging.info("Receiving: %s\n"
                 "Body: %s\n",
                 receivedmessage,
                 receivedmessage.body)
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- Confirm that the parameters within the trigger reflect values that correspond with your storage account.
- The name of the file must be `function_app.py`.
- If you are using SDK-Type Bindings, make sure to include `azurefunctions-extensions-bindings-servicebus` in your `requirements.txt` file.
  
Note that Service Bus output bindings are also supported in Azure Functions. To learn more, see [Azure Service Bus bindings for Azure Functions](https://aka.ms/azure-function-binding-service-bus)

## V2 Programming Model

The v2 programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

To learn more about using the Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.