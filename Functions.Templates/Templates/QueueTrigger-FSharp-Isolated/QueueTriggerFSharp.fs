namespace Company.Function

open System
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging

module QueueTriggerFSharp =
    [<Function("QueueTriggerFSharp")>]
    let run
        (
            [<QueueTrigger("PathValue", Connection = "ConnectionValue")>] myQueueItem: string,
            context: FunctionContext
        ) =
        let msg =
            sprintf "F# Queue trigger function processed: %s" myQueueItem

        let logger = context.GetLogger "QueueTriggerFSharp"
        log.LogInformation msg
