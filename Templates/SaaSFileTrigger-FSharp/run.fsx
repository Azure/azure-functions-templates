let Run(input: string, name: string, output: byref<string>, log: TraceWriter) =
    log.Info(sprintf "F# SaaS trigger function processed file: %s" name)
    output <- input
