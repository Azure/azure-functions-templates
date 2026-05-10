using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class McpPromptFunction
{
    private ILogger<McpPromptFunction> _logger;

    public McpPromptFunction(ILogger<McpPromptFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(McpPromptFunction))]
    public string Run(
        [McpPromptTrigger(
            "summarize",
            Title = "Summarize Text",
            Description = "Generates a prompt that summarizes the provided text.")]
        PromptInvocationContext context,
        [McpPromptArgument("text", "The text to summarize", isRequired: true)] string? text)
    {
        _logger.LogInformation("MCP Prompt Function triggered.");
        return $"Please provide a concise summary of the following text:\n\n{text ?? "(no text provided)"}";
    }
}
