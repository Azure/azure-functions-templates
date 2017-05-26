open System

let Run(myTimer: TimerInfo, log: TraceWriter) =
    log.Info(
        sprintf "F# Timer trigger function executed at: %s" 
            (DateTime.Now.ToString()))
