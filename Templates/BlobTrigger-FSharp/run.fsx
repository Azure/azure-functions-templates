open System

let Run(myBlob: Stream, name: string, log: TraceWriter) =
    log.Verbose(sprintf "F# Blob trigger function processed blob\n Name: %s \n Size: %d Bytes" name myBlob.Length)
