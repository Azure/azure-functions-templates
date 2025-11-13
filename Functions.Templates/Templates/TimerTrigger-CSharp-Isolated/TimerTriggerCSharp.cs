using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class TimerTriggerCSharp(ILogger<TimerTriggerCSharp> logger)
{
    [Function("TimerTriggerCSharp")]
    public void Run([TimerTrigger("ScheduleValue")] TimerInfo myTimer)
    {
        logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);
        
        if (myTimer.ScheduleStatus is not null)
        {
            logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}