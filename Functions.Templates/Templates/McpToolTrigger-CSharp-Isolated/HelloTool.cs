using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class HelloTool(ILogger<HelloTool> logger)
{
    [Function(nameof(HelloTool))]
    public string Run(
        [McpToolTrigger(nameof(HelloTool), "Responds to the user with a hello message.")] ToolInvocationContext context,
        [McpToolProperty(nameof(name), "The name of the person to greet.")] string? name
    )
    {
        logger.LogInformation("C# MCP tool trigger function processed a request.");
        return $"Hello, {name ?? "world"}! This is an MCP Tool!";
    }
}
