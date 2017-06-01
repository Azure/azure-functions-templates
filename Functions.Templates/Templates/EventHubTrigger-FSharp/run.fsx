open System

let Run(myEventHubMessage: string, log: TraceWriter) =
    log.Info(sprintf "F# Event Hub trigger function processed a message: %s" myEventHubMessage)
