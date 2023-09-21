# Azure Functions: Event Hub Trigger in Python

## Dapr Output Binding

Using `Dapr Topic Trigger`, your azure functions can react to a message published on a Topic mentioned in your function.

## Using the Template

Following is an example code snippet for Dapr Topic Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel) (currently in Preview).

```python
import json
import azure.functions as func
import logging

dapp = func.DaprFunctionApp(http_auth_level=func.AuthLevel.ANONYMOUS)

@dapp.function_name(name="DaprTopicTriggerPython")
@dapp.dapr_topic_trigger(arg_name="subEvent", pub_sub_name="pubsub", topic="A")
@dapp.dapr_state_output(arg_name="state", state_store="statestore", key="order")
def main(subEvent, state: func.Out[str]) -> None:
    logging.info('Python DaprTopic trigger with DaprState output binding function processed a request from the Dapr Runtime.')
    subEvent_json = json.loads(subEvent)
    state.set(str(subEvent_json["data"]))
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `dapp`.
- The name of the file must be `function_app.py`.

## V2 Programming Model

The v2 programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

To learn more about using the Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.