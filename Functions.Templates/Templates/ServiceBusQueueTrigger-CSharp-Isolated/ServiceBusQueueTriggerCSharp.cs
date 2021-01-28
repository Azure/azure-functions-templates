using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Abstractions;
using Microsoft.Azure.Functions.Worker.Extensions.ServiceBus;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class ServiceBusQueueTriggerCSharp
    {
        [FunctionName("ServiceBusQueueTriggerCSharp")]
        public static void Run([ServiceBusTrigger("QueueNameValue", Connection = "ConnectionValue")] string myQueueItem, FunctionContext context)
        {
            var logger = context.Logger;
            logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
