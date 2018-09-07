namespace Company.Function

open System
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Host
open Microsoft.Extensions.Logging

module QueueTriggerFSharp =
    [<FunctionName("QueueTriggerCSharp")>]
    let run([<QueueTrigger("PathValue", Connection = "ConnectionValue")>]myQueueItem: string, log: ILogger) =
        let msg = sprintf "C# Queue trigger function processed: %s" myQueueItem
        log.LogInformation msg