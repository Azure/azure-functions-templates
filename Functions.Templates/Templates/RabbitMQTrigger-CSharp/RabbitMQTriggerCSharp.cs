using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class RabbitMQTriggerCSharp
    {
        [FunctionName("RabbitMQTriggerCSharp")]
        public void Run([RabbitMQTrigger("NameOfQueue", ConnectionStringSetting = "ConnectionValue")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
