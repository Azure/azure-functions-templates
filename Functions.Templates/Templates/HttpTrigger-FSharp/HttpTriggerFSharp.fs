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
    [<FunctionName("HttpTriggerFSharp")>]
    let run ([<HttpTrigger(AuthorizationLevel.AuthLevelValue, "get", "post", Route = null)>]req: HttpRequest) (log: ILogger) =
        log.LogInformation("C# HTTP trigger function processed a request.")

        let name = req.Query.["name"].[0]

        if not(isNull name) then
            OkObjectResult(sprintf "Hello, %s" name) :> ActionResult
        else
            BadRequestObjectResult("Please pass a name on the query string or in the request body") :> ActionResult