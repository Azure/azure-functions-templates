namespace Company.Function
// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

open System
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging

module EventGridTriggerFSharp =
    type MyEvent =
        { Id: string
          Topic: string
          Subject: string
          EventType: string
          EventTime: DateTime
          Data: object }

    [<Function("EventGridTriggerFSharp")>]
    let run ([<EventGridTrigger>] eventGridEvent: MyEvent, context: FunctionContext) =
        let logger =
            context.GetLogger("EventGridTriggerCSharp")

        log.LogInformation(eventGridEvent.Data.ToString())
