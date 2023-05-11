# Azure Functions: Event Grid Trigger in Python

## Event Grid Trigger

The Event Grid function trigger can be used to respond to an event sent by an Event Grid source. You must have an event subscription to the source to receive events. When the function is triggered, it converts the event data into a JSON string which is then logged using the Python logging module.

## Using the Template
Following is an example code snippet for Event Grid Trigger using the Python programming model V2 (currently in Preview).
```python
import azure.functions as func
import logging
import json

app = func.FunctionApp()

@app.function_name(name="eventgridtrigger")
@app.event_grid_trigger(arg_name="event")
def test_function(event: func.EventGridEvent):

    result = json.dumps({
        'id': event.id,
        'data': event.get_json(),
        'topic': event.topic,
        'subject': event.subject,
        'event_type': event.event_type,
    })

    logging.info('Python EventGrid trigger processed an event: %s', result)
```
To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- Confirm that the parameters within the trigger reflect values that correspond with your storage account.
- The name of the file must be `function_app.py`.

## Programming Model V2 (Preview)

The new programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

The improved programming model requires fewer files than the default model, and specifically eliminates the need for a configuration file (`function.json`). Instead, triggers and bindings are represented in the `function_app.py` file as decorators. Moreover, functions can be logically organized with support for multiple functions to be stored in the same file. Functions within the same function application can also be stored in different files, and be referenced as blueprints.

To learn more about using the new Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.

To learn more about the new programming model for Azure Functions in Python, see [Programming Models in Azure Functions](https://aka.ms/functions-programming-models).