namespace Company.Function

open System
open System.Text
open System.Threading.Tasks
open Microsoft.Azure.EventHubs
open Microsoft.Azure.WebJobs
open Microsoft.Extensions.Logging

module EventHuBTriggerFSharp =
    [<FunctionName("EventHubTriggerFSharp")>]
    let run([<EventHubTrigger("eventHubNameValue", Connection="ConnectionValue")>] events: EventData[], log: ILogger) =
        async {
            let exns = ResizeArray<Exception>()

            for eventData in events do
                try
                    let msgBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count)
                    let msg = sprintf "F# Event Hub trigger function processed a message: %s" msgBody
                    log.LogInformation msg
                    Task.Yield() |> Async.AwaitTask
                with
                | e -> exns.Add(e)

            if exns.Count > 1 then
                raise(AggregateException(exns))
            if exns.Count = 1 then
                raise(exns.[0])
        } |> Async.StartAsTask
