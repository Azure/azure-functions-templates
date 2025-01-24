using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class TimerTriggerCSharp
{
    private readonly ILogger _logger;

    public TimerTriggerCSharp(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<TimerTriggerCSharp>();
    }

    [Function("TimerTriggerCSharp")]
    public void Run([TimerTrigger("ScheduleValue")] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);
        
        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}