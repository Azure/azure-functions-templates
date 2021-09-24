using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class TimerTriggerCSharp
    {
        private readonly ILogger<TimerTriggerCSharp> _logger;

        public TimerTriggerCSharp(FunctionContext context)
        {
            _logger = context.GetLogger("TimerTriggerCSharp");
        }

        [Function("TimerTriggerCSharp")]
        public static void Run([TimerTrigger("ScheduleValue")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
