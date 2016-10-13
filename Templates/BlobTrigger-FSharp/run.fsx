open System

let Run(myBlob: string, log: TraceWriter) =
    log.Verbose(sprintf "F# Blob trigger function processed blob: %s" myBlob)
