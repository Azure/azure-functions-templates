namespace Company.Function

open System
open Azure.Storage.Queues.Models;
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging

module QueueTriggerFSharp =
    [<Function("QueueTriggerFSharp")>]
    let run
        (
            [<QueueTrigger("PathValue", Connection = "ConnectionValue")>] message: QueueMessage,
            context: FunctionContext
        ) =
        let msg =
            sprintf "F# Queue trigger function processed: %s" message.MessageText

        let logger = context.GetLogger "QueueTriggerFSharp"
        log.LogInformation msg
