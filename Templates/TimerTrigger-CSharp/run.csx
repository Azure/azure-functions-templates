using System;

public static void Run(TimerInfo myTimer, TraceWriter log)
{
    log.Verbose($"C# Timer trigger function executed at: {DateTime.Now}");    
}