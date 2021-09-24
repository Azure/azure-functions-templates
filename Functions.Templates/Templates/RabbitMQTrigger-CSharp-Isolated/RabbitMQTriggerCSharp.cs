using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class RabbitMQTriggerCSharp
    {
        private readonly ILogger<QueueTriggerCSharp> _logger;

        public QueueTriggerCSharp(FunctionContext context)
        {
            _logger = context.GetLogger("RabbitMQTriggerCSharp");
        }

        [Function("RabbitMQTriggerCSharp")]
        public void Run([RabbitMQTrigger("NameOfQueue", ConnectionStringSetting = "ConnectionValue")] string myQueueItem)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
