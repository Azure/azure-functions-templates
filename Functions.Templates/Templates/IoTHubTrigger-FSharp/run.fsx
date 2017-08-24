open System

let Run(myIoTHubMessage: string, log: TraceWriter) =
    log.Info(sprintf "F# IoT Hub trigger function processed a message: %s" myIoTHubMessage)
