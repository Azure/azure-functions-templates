# Azure Functions: Event Hub Trigger in Python

## Dapr Output Binding

With Dapr output binding, you can invoke external resources. An optional payload and metadata can be sent with the invocation request.

## Using the Template

Following is an example code snippet for Dapr Service Invocation Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel) (currently in Preview).

```python
import datetime
import json
import azure.functions as func
import logging

app = func.FunctionApp(http_auth_level=func.AuthLevel.ANONYMOUS)

@app.function_name(name="DaprPublishOutputBindingPython")
@app.timer_trigger(schedule="*/10 * * * * *", arg_name="myTimer", run_on_startup=False)
@app.dapr_publish_output(arg_name="pubEvent", pub_sub_name="pubsub", topic="A")
def main(myTimer, pubEvent: func.Out[bytes]) -> None:
    logging.info('Python DaprPublish output binding function processed a request.')
    payload = f"Invoked by Timer trigger: Hello, World! The time is {datetime.datetime.now()}"
    pubEvent.set(json.dumps({"payload": payload}).encode('utf-8'))
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- The name of the file must be `function_app.py`.

## V2 Programming Model

The v2 programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

To learn more about using the Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.