using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class McpResourceFunction
{
    private ILogger<McpResourceFunction> _logger;

    private const string ResourceMetadata = """
        {
            "author": "John Doe",
            "file": {
                "version": 1.0,
                "releaseDate": "2026-01-01"
            },
        }
        """;

    public McpResourceFunction(ILogger<McpResourceFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(McpResourceFunction))]
    public string Run(
        [McpResourceTrigger(
            "file://readme.md",
            "readme",
            Description = "Application readme file",
            MimeType = "text/plain")]
        [McpMetadata(ResourceMetadata)] ResourceInvocationContext context)
    {
        _logger.LogInformation("MCP Resource Function triggered.");
        return "# Sample Readme\nThis is a sample readme file for testing MCP Resource Trigger.";
    }
}
