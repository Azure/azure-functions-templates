#r "System.Net.Http"

open System.Net
open System.Net.Http

let Run(req: HttpRequestMessage, log: TraceWriter) =
    log.Info(sprintf 
        "F# HTTP trigger function processed a request.")

    // Set name to query string
    let name =
        req.GetQueryNameValuePairs()
        |> Seq.tryFind (fun q -> q.Key = "name")
        |> fun x -> x.Value
    
    req.CreateResponse(HttpStatusCode.OK, "Hello " + name.Value);