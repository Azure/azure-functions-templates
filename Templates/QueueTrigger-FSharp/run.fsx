open System

let Run (input: string, log: TraceWriter) =  
    log.Info(sprintf "F# Queue trigger function processed: '%s'" input)