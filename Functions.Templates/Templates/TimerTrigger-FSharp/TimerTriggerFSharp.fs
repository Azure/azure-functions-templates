namespace Company.Function

open System
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Host
open Microsoft.Extensions.Logging

module TimerTriggerFSharp =
    [<FunctionName("TimerTriggerCSharp")>]
    let run([<TimerTrigger("ScheduleValue")>]myTimer: TimerInfo, log: ILogger) =
        let msg = sprintf "F# Time trigger function executed at: %A" DateTime.Now
        log.LogInformation msg
