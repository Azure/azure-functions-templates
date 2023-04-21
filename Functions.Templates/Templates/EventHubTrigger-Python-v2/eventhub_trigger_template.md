# Azure Functions: Event Hub Trigger in Python

## Event Hub Trigger

The Event Hub function trigger can be used to respond to an event sent to an event hub event stream. You must have read access to the underlying event hub to set up the trigger. When the function is triggered, the message passed to the function is typed as a string.

## Using the Template

Following is an example code snippet for Event Hub Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel) (currently in Preview).

```python
import logging
import azure.functions as func

app = func.FunctionApp()

@app.function_name(name="EventHubTrigger1")
@app.event_hub_message_trigger(arg_name="myhub", event_hub_name="samples-workitems",
                               connection="<CONNECTION_SETTING>") 
def test_function(myhub: func.EventHubEvent):
    logging.info('Python EventHub trigger processed an event: %s',
                myhub.get_body().decode('utf-8'))
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- Confirm that the parameters within the trigger reflect values that correspond with your storage account.
- The name of the file must be `function_app.py`.
  
Note that Event Hub output bindings are also supported in Azure Functions. To learn more, see [Azure Event Hubs trigger and bindings for Azure Functions](https://aka.ms/azure-function-binding-event-hubs)

## V2 Programming Model

The v2 programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

To learn more about using the Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.