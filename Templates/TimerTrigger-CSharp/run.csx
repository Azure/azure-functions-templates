using System;
using System.Diagnostics;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

public static void Run(TimerInfo myTimer, TraceWriter log)
{
    log.Verbose(string.Format("C# Timer trigger function executed at {0}:", DateTime.Now.ToString()));    
}