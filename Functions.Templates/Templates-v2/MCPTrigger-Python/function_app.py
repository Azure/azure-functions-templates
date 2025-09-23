import azure.functions as func
import logging

app = func.FunctionApp(http_auth_level=func.AuthLevel.$(AUTHLEVEL_INPUT))

@app.mcp_tool_trigger(
    arg_name="context",
    type="mcpToolTrigger",
    tool_name="hello_mcp",
    description="Hello world.",
    toolProperties="[]",
)
def $(FUNCTION_NAME_INPUT)(context) -> None:
    return "Hello I am MCPTool!"
