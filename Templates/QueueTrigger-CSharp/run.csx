using System;
using System.Threading.Tasks;

public static void Run(string myQueueItem, TraceWriter log)
{
    log.Verbose($"C# Queue trigger function processed: {myQueueItem}");
}