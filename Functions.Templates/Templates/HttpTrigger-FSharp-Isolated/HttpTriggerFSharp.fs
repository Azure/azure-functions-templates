namespace Company.Function

open System.Net
open System.Threading.Tasks
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Logging

module HttpTriggerFSharp =

    [<Function("HttpTriggerFSharp")>]
    let run
        ([<HttpTrigger(AuthorizationLevel.AuthLevelValue, "get", "post", Route = null)>] req: HttpRequestData)
        (context: FunctionContext)
        : Task<HttpResponseData> =
        task {
            let logger = context.GetLogger "HttpTriggerFSharp"
            logger.LogInformation "F# HTTP trigger function processed a request"

            let response = req.CreateResponse(HttpStatusCode.OK)
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8")

            do! response.WriteStringAsync "Welcome to Azure Functions!"

            return response
        }
