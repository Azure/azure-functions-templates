# Azure Functions: Cosmos DB in Python

## Dapr Service Invocation Trigger

Using service invocation, your application can reliably and securely communicate with other applications using HTTP protocols.

## Using the Template

Following is an example code snippet for Dapr Service Invocation Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel) (currently in Preview).

```python
import json
import azure.functions as func
import logging

app = func.FunctionApp(http_auth_level=func.AuthLevel.ANONYMOUS)

@app.function_name(name="DaprServiceInvocationTriggerPython")
@app.dapr_service_invocation_trigger(arg_name="payload", method_name="DaprServiceInvocationTriggerPython")
def main(payload: str) :
    logging.info('Azure function triggered by Dapr Service Invocation Trigger.')
    logging.info("Dapr service invocation trigger payload: %s", payload)
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- The name of the file must be `function_app.py`.

## V2 Programming Model

The v2 programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

To learn more about using the Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.