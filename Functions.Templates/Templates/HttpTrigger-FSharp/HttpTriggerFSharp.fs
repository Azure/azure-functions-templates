namespace Company.Function

open System.IO
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Host
open Newtonsoft.Json
open Microsoft.Extensions.Logging

module HttpTriggerFSharp =
    [<CLIMutable>]
    type NameContainer = {Name:string}

    [<Literal>]
    let Name = "Name"

    [<FunctionName("HttpTriggerFSharp")>]
    let run ([<HttpTrigger(AuthorizationLevel.AuthLevelValue, "get", "post", Route = null)>]req: HttpRequest) (log: ILogger) =
        log.LogInformation("F# HTTP trigger function processed a request.")

    let badRequest = BadRequestObjectResult "Please pass a name on the query string or in the request body" :> IActionResult

    let deserializeNameContainer body= 
        try
            Some <| JsonConvert.DeserializeObject<NameContainer>(body)
        with
        | :? JsonReaderException -> None

    let name = 
        match req.Query.ContainsKey Name with
        | true -> Some req.Query.[Name].[0]
        | false -> None

    let reqBodyName = 
        async {
        use stream = new StreamReader(req.Body)
        let! body = stream.ReadToEndAsync() |> Async.AwaitTask
        let input = 
            match body with
            | b when System.String.IsNullOrWhiteSpace b -> None
            | _ -> deserializeNameContainer body

        match input with
        | None -> 
            return badRequest
        | Some n ->
            log.LogInformation n.Name
            match n.Name with
            | null | "" -> return badRequest
            | _ -> 
                return OkObjectResult (sprintf "Hello, %s" n.Name) :>IActionResult
        }
    

    match name with
    | Some n ->
        OkObjectResult(sprintf "Hello, %s" n) :>IActionResult
    | None ->
        reqBodyName |> Async.RunSynchronously