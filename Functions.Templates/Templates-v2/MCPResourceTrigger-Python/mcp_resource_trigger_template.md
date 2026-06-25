# Azure Functions: MCP Resource Trigger in Python

## MCP Resource Trigger

The MCP (Model Context Protocol) Resource Trigger decorator enables your function to be triggered when MCP resource requests are received by the host. This decorator allows you to expose resources with metadata that can be discovered and accessed through the MCP protocol.

The decorator:
- Registers the resource with a unique URI identifier
- Exposes resource metadata including name, description, and MIME type
- Handles MCPToolContext injection
- Enables resource content delivery through function return values

For more information, see the [Remote MCP Functions Python documentation](https://aka.ms/remote-mcp-functions-python).

## Using the Template

Following are example code snippets for MCP Resource Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel).

### Simple Example

```python
import azure.functions as func
import logging

app = func.FunctionApp()

RESOURCE_METADATA = """
        {
            "author": "John Doe",
            "file": {
                "version": 1.0,
                "releaseDate": "2026-01-01"
            }
        }
        """

@app.mcp_resource_trigger(
    arg_name="context",
    uri="file://readme.md",
    resource_name="readme",
    description="Application readme file",
    mime_type="text/plain",
    metadata=RESOURCE_METADATA
)
def my_mcp_resource(context: func.MCPToolContext) -> str:
    logging.info("MCP Resource Function triggered with context: %s", context)
    return "# Sample Readme\nThis is a sample readme file for testing MCP Resource Trigger."
```

## Key Parameters

- **uri**: Unique URI identifier for the resource (must be absolute, e.g., `file://readme.md`, `config://settings`)
- **resource_name**: Human-readable name of the resource
- **description**: Optional description of what the resource contains
- **mime_type**: Optional MIME type (e.g., `text/plain`, `application/json`, `text/markdown`)
- **metadata**: Optional JSON-serialized metadata object for additional resource information
- **title**: Optional title for display purposes
- **size**: Optional size of the resource in bytes

To run the code snippets generated through the command palette, note the following:

- The function application is defined and named `app`.
- The name of the file must be `function_app.py`.
- Ensure you are using `azure-functions` version 1.25.0b3 or greater.
- The function return value will be the content delivered when the resource is accessed.

## V2 Programming Model

The v2 programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

To learn more about using the Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.
