namespace Company.Function

// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

open System
open Azure.Messaging;
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging

module EventGridTriggerFSharp =
    [<Function("EventGridTriggerFSharp")>]
    let run ([<EventGridTrigger>] cloudEvent: CloudEvent, context: FunctionContext) =
        let logger =
            context.GetLogger("EventGridTriggerFSharp")

        let msg =
            sprintf "Event type: %s, Event subject: %s" cloudEvent.Type cloudEvent.Subject

        log.LogInformation msg
