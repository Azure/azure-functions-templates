using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Abstractions;
using Microsoft.Azure.Functions.Worker.Extensions.RabbitMQ;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class RabbitMQTriggerCSharp
    {
        [FunctionName("RabbitMQTriggerCSharp")]
        public static void Run([RabbitMQTrigger("NameOfQueue", ConnectionStringSetting = "ConnectionValue")] string myQueueItem, FunctionContext context)
        {
            var logger = context.Logger;
            logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
