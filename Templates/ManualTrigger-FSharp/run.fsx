let Run(input: string, log: TraceWriter) =
    log.Info(
        sprintf "F# manually triggered function called with input: %s" input)
