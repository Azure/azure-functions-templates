#r "System.Net.Http"

open System.Net
open System.Net.Http
open FSharp.Interop.Dynamic

let Run(req: HttpRequestMessage, log: TraceWriter) =
    log.Info(sprintf 
        "F# HTTP trigger function processed a request. RequestUri=%s" 
        (req.RequestUri.ToString()))

    // Set name to query string or body data
    let name =
        req.GetQueryNameValuePairs()
        |> Seq.tryFind (fun q -> q.Key = "name")
        |> function
            | Some q -> q.Value
            | None ->
                async {
                    let! data =
                        req.Content.ReadAsAsync<obj>()
                        |> Async.AwaitTask
                    return data?name
                } |> Async.RunSynchronously

    if isNull name then
        req.CreateResponse(HttpStatusCode.BadRequest,
            "Please pass a name on the query string or in the request body")
    else
        req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
