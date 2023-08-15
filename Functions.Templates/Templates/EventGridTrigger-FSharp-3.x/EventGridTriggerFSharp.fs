namespace Company.Function
// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Host
open Microsoft.Azure.EventGrid.Models
open Microsoft.Azure.WebJobs.Extensions.EventGrid
open Microsoft.Extensions.Logging
open Azure.Messaging

module EventGridTriggerFSharp =
    [<Functionname>]
    let run ([<EventGridTrigger>] eventGridEvent: EventGridEvent, log: ILogger) =
        log.LogInformation(eventGridEvent.Data.ToString())