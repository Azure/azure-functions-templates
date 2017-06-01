let Run(myQueueItem: string, log: TraceWriter) =
    log.Info(sprintf "F# ServiceBus function processed message: %s" myQueueItem)
