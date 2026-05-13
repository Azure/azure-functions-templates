# Azure Functions: MCP Prompt Trigger in Python

## MCP Prompt Trigger

The MCP (Model Context Protocol) Prompt Trigger decorator enables your function to be triggered when MCP prompt requests are received by the host. This decorator allows you to expose reusable prompts that can be discovered and invoked through the MCP protocol.

The decorator:
- Registers the prompt with a unique name identifier
- Exposes prompt metadata including description
- Handles PromptInvocationContext injection
- Enables prompt content delivery through function return values

For more information, see the [Remote MCP Functions Python documentation](https://aka.ms/remote-mcp-functions-python).

## Using the Template

Following are example code snippets for MCP Prompt Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel).

### Simple Example

```python
import azure.functions as func
import logging

app = func.FunctionApp(http_auth_level=func.AuthLevel.ANONYMOUS)

@app.mcp_prompt_trigger(
    arg_name="context",
    prompt_name="code_review_checklist",
    description="Returns a structured code review checklist prompt for evaluating code changes."
)
def code_review_checklist(context: func.PromptInvocationContext) -> str:
    logging.info("Code review checklist prompt invoked.")
    
    return """You are a senior software engineer performing a code review.
Use the following checklist to evaluate the code:

1. **Correctness** — Does the code do what it's supposed to?
2. **Error Handling** — Are edge cases and failures handled?
3. **Security** — Are there any vulnerabilities (injection, auth, secrets)?
4. **Performance** — Are there obvious inefficiencies?
5. **Readability** — Is the code clear and well-named?
6. **Tests** — Are there adequate tests for the changes?

Provide your feedback in a structured format with a severity level
(critical, warning, suggestion) for each finding."""
```

## Key Parameters

- **arg_name**: The name of the argument that will receive the PromptInvocationContext
- **prompt_name**: Unique identifier for the prompt within the MCP server
- **description**: Human-readable description of what the prompt does

## Context Object

The `PromptInvocationContext` provides information about the prompt invocation and can be used to access request metadata.

## Return Value

The function should return a string containing the prompt text. This can be:
- Static prompt templates
- Dynamically generated prompts based on context
- Multi-line prompts with formatting

## Common Use Cases

- Code review templates
- Documentation generation prompts
- Testing strategy guides
- Architecture decision templates
- Best practices checklists
- Troubleshooting workflows
