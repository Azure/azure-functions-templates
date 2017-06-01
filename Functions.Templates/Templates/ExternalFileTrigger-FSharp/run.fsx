let Run(input: string, name: string, log: TraceWriter) =
    log.Info(sprintf "F# External trigger function processed file: %s" name)
    input
