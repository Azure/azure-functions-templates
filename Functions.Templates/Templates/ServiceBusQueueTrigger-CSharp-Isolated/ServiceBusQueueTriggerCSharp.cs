using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class ServiceBusQueueTriggerCSharp
    {
        [Function("ServiceBusQueueTriggerCSharp")]
        public static void Run([ServiceBusTrigger("QueueNameValue", Connection = "ConnectionValue")] string myQueueItem, FunctionContext context)
        {
            var logger = context.GetLogger("ServiceBusQueueTriggerCSharp");
            logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
