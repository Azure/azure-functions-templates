# Azure Functions: MCP Tool in Python

## MCP Tool

The MCP (Model Context Protocol) tool decorator allows you to register an MCP tool function in Azure Functions. This decorator automatically:
- Infers tool name from function name
- Extracts docstrings as description
- Extracts parameters and types for tool properties
- Handles MCPToolContext injection

For more information, see the [Remote MCP Functions Python documentation](https://aka.ms/remote-mcp-functions-python).

## Using the Template

Following are example code snippets for MCP Tool using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel).

### Simple Example

```python
import azure.functions as func

app = func.FunctionApp(http_auth_level=func.AuthLevel.ANONYMOUS)

@app.mcp_tool()
def my_mcp_tool(context: func.MCPToolContext) -> None:
    """
    A simple function that returns a greeting message.
    """
    return "Hello I am MCPTool! Called with context: " + str(context)
```

### Detailed Example with Parameters

```python
import logging
import azure.functions as func

app = func.FunctionApp()

@app.function_name(name="MCPTool1")
@app.mcp_tool()
def my_tool(param1: str, param2: int) -> str:
    """This is my MCP tool description.
    
    Args:
        param1: Description of parameter 1
        param2: Description of parameter 2
    
    Returns:
        A string result
    """
    logging.info(f"MCP tool called with param1={param1}, param2={param2}")
    return f"Processed: {param1} - {param2}"
```

To run the code snippets generated through the command palette, note the following:

- The function application is defined and named `app`.
- The name of the file must be `function_app.py`.
- Ensure you are using `azure-functions` version 1.25.0b2 or greater.

## V2 Programming Model

The v2 programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

To learn more about using the Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.
