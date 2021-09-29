using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class RabbitMQTriggerCSharp
    {
        private readonly ILogger<RabbitMQTriggerCSharp> _logger;

        public RabbitMQTriggerCSharp(ILogger<RabbitMQTriggerCSharp> log)
        {
            _logger = log;
        }

        [FunctionName("RabbitMQTriggerCSharp")]
        public void Run([RabbitMQTrigger("NameOfQueue", ConnectionStringSetting = "ConnectionValue")]string myQueueItem)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
