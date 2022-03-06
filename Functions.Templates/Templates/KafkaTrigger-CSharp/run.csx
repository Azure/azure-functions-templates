#r "Microsoft.Azure.WebJobs.Extensions.Kafka"

using System;
using System.Text;
using Microsoft.Azure.WebJobs.Extensions.Kafka;

public static void Run(KafkaEventData<string> eventData, ILogger log)
{
    log.LogInformation($"C# Queue trigger function processed: {eventData.Value}");
}
