using System;

public static void Run(string myQueueItem, TraceWriter log)
{
    log.Verbose($"C# Queue trigger function processed: {myQueueItem}");
}