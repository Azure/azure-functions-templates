using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class RabbitMQTriggerCSharp(ILogger<RabbitMQTriggerCSharp> logger)
{
    [Function("RabbitMQTriggerCSharp")]
    public void Run([RabbitMQTrigger("NameOfQueue", ConnectionStringSetting = "ConnectionValue")] string myQueueItem)
    {
        logger.LogInformation("C# Queue trigger function processed: {item}", myQueueItem);
    }
}