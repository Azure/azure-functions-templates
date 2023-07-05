# Durable Functions in Python Programming Model V2

## Durable Functions

Durable Functions is an extension of Azure Functions that lets you write stateful functions in a serverless compute environment. The extension lets you define stateful workflows by writing orchestrator functions and stateful entities by writing entity functions using the Azure Functions programming model. Behind the scenes, the extension manages state, checkpoints, and restarts for you, allowing you to focus on your business logic.

## Using the Template

The following is an example code snippet showing an activity trigger for a Durable Functions app using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel).

```python
import azure.functions as func
import azure.durable_functions as df

dfApp = df.DFApp(http_auth_level=func.AuthLevel.ANONYMOUS)

# Activity
@dfApp.activity_trigger(input_name="city")
def hello(city: str):
    return "Hello " + city  
```

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `dfApp`.
- The name of the file must be `function_app.py`.

After starting the app, send the HTTP request with replacing {functionName} to the orchestration function name to trigger the orchestrator.

## Programming Model V2

The new programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks.

The improved programming model requires fewer files than the default model, and specifically eliminates the need for a configuration file (`function.json`). Instead, triggers and bindings are represented in the `function_app.py` file as decorators. Moreover, functions can be logically organized with support for multiple functions to be stored in the same file. Functions within the same function application can also be stored in different files, and be referenced as blueprints.

To learn more about using the new Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.

To learn more about the new programming model for Azure Functions in Python, see [Programming Models in Azure Functions](https://aka.ms/functions-programming-models).