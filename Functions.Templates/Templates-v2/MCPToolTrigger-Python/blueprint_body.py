@$(BLUEPRINT_FILENAME).mcp_tool()
def $(FUNCTION_NAME_INPUT)(context: func.MCPToolContext) -> None:
    """
    A simple function that returns a greeting message.
    """
    return "Hello I am MCPTool! Called with context: " + str(context)