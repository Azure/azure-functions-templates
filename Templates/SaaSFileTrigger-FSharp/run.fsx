let Run(input: string, name: string, log: TraceWriter) =
    log.Info(sprintf "F# SaaS trigger function processed file: %s" name)
    input
