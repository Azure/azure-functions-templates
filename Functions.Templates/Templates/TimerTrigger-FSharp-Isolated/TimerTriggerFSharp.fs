namespace Company.Function

open System
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging

module TimerTriggerFSharp =
    type MyScheduleStatus =
        { Last: DateTime
          Next: DateTime
          LastUpdate: DateTime }

    type MyInfo =
        { ScheduleStatus: MyScheduleStatus
          IsPastDue: bool }

    [<Function("TimerTriggerFSharp")>]
    let run ([<TimerTrigger("ScheduleValue")>] myTimer: MyInfo, context: FunctionContext) =
        let logger = context.GetLogger "TimerTriggerFSharp"
        logger.LogInformation(sprintf "F# Time trigger function executed at: %A" DateTime.Now)
        logger.LogInformation(sprintf "Next timer schedule at: %A" myTimer.ScheduleStatus.Next)
