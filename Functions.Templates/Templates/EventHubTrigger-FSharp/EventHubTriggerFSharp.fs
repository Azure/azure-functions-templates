namespace Company.Function

open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Host
open Microsoft.Extensions.Logging

module EventHuBTriggerFSharp =
    [<FunctionName("EventHubTriggerFSharp")>]
    let run([<EventHubTrigger("eventHubNameValue", Connection="ConnectionValue")>]myEventHubMessage: string, log: ILogger) =
        let msg = sprintf "F# Event Hub trigger function processed a message: %s" myEventHubMessage
        log.LogInformation msg
