using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class RabbitMQTriggerCSharp
{
    private readonly ILogger _logger;

    public RabbitMQTriggerCSharp(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<RabbitMQTriggerCSharp>();
    }

    [Function("RabbitMQTriggerCSharp")]
    public void Run([RabbitMQTrigger("NameOfQueue", ConnectionStringSetting = "ConnectionValue")] string myQueueItem)
    {
        _logger.LogInformation("C# Queue trigger function processed: {item}", myQueueItem);
    }
}