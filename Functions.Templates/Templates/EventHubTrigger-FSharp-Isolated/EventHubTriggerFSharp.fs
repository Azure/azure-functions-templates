namespace Company.Function

open System
open Azure.Messaging.EventHubs;
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging

module EventHuBTriggerFSharp =
    [<Function("EventHubTriggerFSharp")>]
    let run
        (
            [<EventHubTrigger("eventHubNameValue", Connection = "ConnectionValue")>] events: EventData [],
            context: FunctionContext
        ) =
        let logger =
            context.GetLogger("EventHubTriggerFSharp")

        for event in events do
            logger.LogInformation $"Event Body: {event.Body}"
            logger.LogInformation $"Event Content-Type: {event.contentType}"
