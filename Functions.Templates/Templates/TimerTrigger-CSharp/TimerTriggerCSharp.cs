#if (portalTemplates)
using System;

public static void Run(TimerInfo myTimer, ILogger log)
#endif
#if (vsTemplates)
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class TimerTriggerCSharp
    {
        [FunctionName("TimerTriggerCSharp")]
        public static void Run([TimerTrigger("ScheduleValue")]TimerInfo myTimer, ILogger log)
#endif
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
#if (vsTemplates)
    }
}
#endif