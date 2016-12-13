#r "System.Net.Http"
#r "Newtonsoft.Json"

open System.Net
open System.Net.Http
open Newtonsoft.Json

type Body = {
    body: string
}

type Comment = {
    comment: Body
}

let Run(req: HttpRequestMessage, log: TraceWriter) =
    async {
        log.Info(sprintf "F# HTTP trigger function processed a request.")

        let! body = req.Content.ReadAsStringAsync() |> Async.AwaitTask
        let data = JsonConvert.DeserializeObject<Comment>(body)
        let comment = data.comment.body

        if comment = null then
            return req.CreateResponse(HttpStatusCode.BadRequest, "No comment data")
        else
            return req.CreateResponse(HttpStatusCode.OK, "From Github: " + comment)
    } |> Async.StartAsTask
