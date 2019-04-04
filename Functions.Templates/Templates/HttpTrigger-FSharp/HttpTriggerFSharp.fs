namespace Company.Function

open System
open System.IO
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Newtonsoft.Json
open Microsoft.Extensions.Logging

module httptrigger1 =
    type NameContainer = { Name: string }
    [<Literal>]
    let Name = "name"

    [<FunctionName("httptrigger1")>]
    let run ([<HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)>]req: HttpRequest) (log: ILogger) =
        async {
            log.LogInformation("F# HTTP trigger function processed a request.")

            let nameOpt = 
                if req.Query.ContainsKey(Name) then
                    Some(req.Query.[Name].[0])
                else
                    None

            use stream = new StreamReader(req.Body)
            let! reqBody = stream.ReadToEndAsync() |> Async.AwaitTask

            let data = JsonConvert.DeserializeObject<NameContainer>(reqBody)
            
            let name =
                match nameOpt with
                | Some n -> n
                | None -> data.Name
            
            if not (String.IsNullOrWhiteSpace(name)) then
                return OkObjectResult(sprintf "Hello, %s" name) :> IActionResult
            else
                return BadRequestObjectResult("Please pass a name on the query string or in the request body") :> IActionResult
        } |> Async.StartAsTask
