using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class RabbitMQTriggerCSharp
    {
        [Function("RabbitMQTriggerCSharp")]
        public static void Run([RabbitMQTrigger("NameOfQueue", ConnectionStringSetting = "ConnectionValue")] string myQueueItem, FunctionContext context)
        {
            var logger = context.GetLogger("RabbitMQTriggerCSharp");
            logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
