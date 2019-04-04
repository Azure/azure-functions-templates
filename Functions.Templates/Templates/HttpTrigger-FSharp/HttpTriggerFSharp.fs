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
    [<Literal>]
    let Name = "name"

    [<FunctionName("HttpTriggerFSharp")>]
    let run ([<HttpTrigger(AuthorizationLevel.AuthLevelValue, "get", "post", Route = null)>]req: HttpRequest) (log: ILogger) =
        async {
            log.LogInformation("F# HTTP trigger function processed a request.")

            let nameOpt = 
                if req.Query.ContainsKey(Name) then
                    Some(req.Query.[Name].[0])
                else
                    None

            let reqBody = StreamReader(req.Body).ReadToEndAsync() |> Async.AwaitTask

            let data = JsonConvert.DeserializeObject(reqBody)
            
            let name =
                match nameOpt with
                | Some n -> n
                | None -> data?name
            
            if String.IsNullOrWhiteSpace(name) then
                OkObjectResult(sprintf "Hello, %s" n) :> IActionResult
            else
                BadRequestObjectResult("Please pass a name on the query string or in the request body") :> IActionResult
        } |> Async.StartAsTask
