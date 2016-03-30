using System;

public static void Run(string myEventHubMessage, TraceWriter log)
{
    log.Verbose($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
}