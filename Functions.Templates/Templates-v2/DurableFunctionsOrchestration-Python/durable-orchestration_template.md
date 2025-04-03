# Azure Functions: Durable Orchestrations in Python

## Durable Functions

Durable Functions is an extension of Azure Functions that lets you write stateful functions in a serverless compute environment. The extension lets you define stateful workflows by writing orchestrator functions and stateful entities by writing entity functions using the Azure Functions programming model. Behind the scenes, the extension manages state, checkpoints, and restarts for you, allowing you to focus on your business logic.

## Using the Template

The following is an example code snippet showing a Durable orchestration in a FunctionApp using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel). The orchestration is comprised of three individual functions - an HTTP starter function, an orchestration function, and an activity function. To learn more, see the [Durable Functions guide for Python](https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview?tabs=in-process%2Cnodejs-v3%2Cv1-model&pivots=python)

```python
import datetime
import logging

import azure.functions as func

app = func.FunctionApp(http_auth_level=func.AuthLevel.ANONYMOUS)

@app.route(route="startOrchestrator")
@app.durable_client_input(client_name="client")
async def start_orchestrator(req: func.HttpRequest, client):
    instance_id = await client.start_new("orchestrator")
    
    logging.info(f"Started orchestration with ID = '{instance_id}'.")
    return client.create_check_status_response(req, instance_id)

@app.orchestration_trigger(context_name="context")
def orchestrator(context: df.DurableOrchestrationContext):
    result1 = yield context.call_activity('say_hello', "Tokyo")
    result2 = yield context.call_activity('say_hello', "Seattle")
    result3 = yield context.call_activity('say_hello', "London")

    yield context.create_timer(context.current_utc_datetime + datetime.timedelta(seconds=5))
    
    return [result1, result2, result3]

@app.activity_trigger(input_name="city")
def say_hello(city: str) -> str:
    return f"Hello {city}!"
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- The name of the file must be `function_app.py`.

After starting the app, send the HTTP request with replacing {functionName} to the orchestration function name to trigger the orchestrator.

## V2 Programming Model

The v2 programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

To learn more about using the Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.