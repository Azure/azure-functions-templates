using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class TimerTriggerCSharp
    {
        private readonly ILogger<TimerTriggerCSharp> _logger;

        public TimerTriggerCSharp(ILogger<TimerTriggerCSharp> log)
        {
            _logger = log;
        }

        [FunctionName("TimerTriggerCSharp")]
        public void Run([TimerTrigger("ScheduleValue")]TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
