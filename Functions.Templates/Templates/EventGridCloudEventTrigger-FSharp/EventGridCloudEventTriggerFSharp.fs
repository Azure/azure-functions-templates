namespace Company.Function
// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Host
open Microsoft.Azure.EventGrid.Models
open Microsoft.Azure.WebJobs.Extensions.EventGrid
open Microsoft.Extensions.Logging

module EventGridTriggerFSharp =
    [<Functionname>]
    let run ([<EventGridTrigger>] eventGridEvent: CloudEvent, log: ILogger) =
        log.LogInformation("Event received {type} {subject}: {data}", cloudEvent.Type, cloudEvent.Subject, cloudEvent.Data.ToString())