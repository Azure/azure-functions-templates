namespace Company.Function

open System.Net
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Logging

module HttpTriggerFSharp =

    [<Function("HttpTriggerFSharp")>]
    let run
        ([<HttpTrigger(AuthorizationLevel.AuthLevelValue, "get", "post", Route = null)>] req: HttpRequestData)
        (context: FunctionContext)
        =
        let logger = context.GetLogger "HttpTriggerFSharp"
        logger.LogInformation "F# HTTP trigger function processed a request"

        let response = req.CreateResponse(HttpStatusCode.OK)
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8")

        response.WriteString "Welcome to Azure Functions!"

        response
