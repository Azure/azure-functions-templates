using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class ServiceBusQueueTriggerCSharp
{
    private readonly ILogger<ServiceBusQueueTriggerCSharp> _logger;

    public ServiceBusQueueTriggerCSharp(ILogger<ServiceBusQueueTriggerCSharp> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ServiceBusQueueTriggerCSharp))]
    public async Task Run(
        [ServiceBusTrigger("QueueNameValue", Connection = "ConnectionValue")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}