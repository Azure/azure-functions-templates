using System;
using System.Threading.Tasks;

public static void Run(string myQueueItem, TraceWriter log)
{
    log.Verbose($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
}