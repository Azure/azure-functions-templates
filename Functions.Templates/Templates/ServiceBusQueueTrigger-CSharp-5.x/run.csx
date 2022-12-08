using System;
using System.Threading.Tasks;

public static void Run(string myQueueItem, ILogger log)
{
    log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
}
