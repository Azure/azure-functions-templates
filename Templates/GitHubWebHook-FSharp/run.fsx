#r "System.Net.Http"

open System.Net
open System.Net.Http
open FSharp.Interop.Dynamic

let Run(req: HttpRequestMessage, log: TraceWriter) =
    async {
        log.Info(sprintf "F# HTTP trigger function processed a request. RequestUri=%s" (req.RequestUri.ToString()))

        let! data = req.Content.ReadAsAsync<obj>() |> Async.AwaitTask
        let comment = data?comment?body

        if isNull comment then
            return req.CreateResponse(HttpStatusCode.BadRequest, "No comment data")
        else
            return req.CreateResponse(HttpStatusCode.OK, "From Github: " + comment)
    } |> Async.StartAsTask
