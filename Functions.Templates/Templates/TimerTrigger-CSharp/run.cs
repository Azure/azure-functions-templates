#if (portalTemplates)
using System;

public static void Run(TimerInfo myTimer, TraceWriter log)
#endif
#if (vsTemplates)
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Company.Function
{
    public static class TimerTriggerCSharp
    {
        [FunctionName("FunctionNameValue")]
        public static void Run([TimerTrigger("ScheduleValue")]TimerInfo myTimer, TraceWriter log)
#endif
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
#if (vsTemplates)
    }
}
#endif