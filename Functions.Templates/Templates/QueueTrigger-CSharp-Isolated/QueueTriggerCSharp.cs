using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class QueueTriggerCSharp(ILogger<QueueTriggerCSharp> logger)
{
    [Function(nameof(QueueTriggerCSharp))]
    public void Run([QueueTrigger("PathValue", Connection = "ConnectionValue")] QueueMessage message)
    {
        logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);
    }
}