namespace Company.Function

open System
open System.Text
open System.Threading.Tasks
open Azure.Messaging.EventHubs
open Microsoft.Azure.WebJobs
open Microsoft.Extensions.Logging

module EventHuBTriggerFSharp =
    [<FunctionName("EventHubTriggerFSharp")>]
    let run([<EventHubTrigger("eventHubNameValue", Connection="ConnectionValue")>] events: EventData[], log: ILogger) =
        async {
            let exns = ResizeArray<Exception>()

            for eventData in events do
                try
                    log.LogInformation $"F# Event Hub trigger function processed a message: {eventData.EventBody}"
                with
                | e -> exns.Add(e)

            if exns.Count > 1 then
                raise(AggregateException(exns))
            if exns.Count = 1 then
                raise(exns[0])
        } |> Async.StartAsTask
