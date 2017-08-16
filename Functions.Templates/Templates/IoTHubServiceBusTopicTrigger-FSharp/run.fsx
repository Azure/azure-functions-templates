let Run(mySbMsg: string, log: TraceWriter) =
    log.Info(sprintf "F# ServiceBus function processed message: %s" mySbMsg)
