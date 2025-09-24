
@$(BLUEPRINT_FILENAME).mcp_tool_trigger(
    arg_name="context",
    type="mcpToolTrigger",
    tool_name="hello_mcp",
    description="Hello world.",
    toolProperties="[]",
)
def $(FUNCTION_NAME_INPUT)(context) -> None:
    return "Hello I am MCPTool!"