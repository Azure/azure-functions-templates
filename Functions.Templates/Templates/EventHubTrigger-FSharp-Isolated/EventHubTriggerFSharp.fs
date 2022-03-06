namespace Company.Function

open System
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging

module EventHuBTriggerFSharp =
    [<Function("EventHubTriggerFSharp")>]
    let run
        (
            [<EventHubTrigger("eventHubNameValue", Connection = "ConnectionValue")>] input: string [],
            context: FunctionContext
        ) =
        let logger =
            context.GetLogger("EventHubTriggerFSharp")

        logger.LogInformation(sprintf "First Event Hubs triggered message: %s" (input |> Array.head))
