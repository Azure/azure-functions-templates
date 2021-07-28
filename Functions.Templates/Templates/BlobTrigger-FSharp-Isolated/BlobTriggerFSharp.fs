namespace Company.Function

open System.IO
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging

module BlobTriggerFSharp =

    [<Function("BlobTriggerFSharp")>]
    let run
        (
            [<BlobTrigger("PathValue/{name}", Connection = "ConnectionValue")>] myBlob: string,
            name: string,
            context: FunctionContext
        ) =
        let msg =
            sprintf "F# Blob trigger function Processed blob\nName: %s \n Data: %s" name myBlob

        let logger = context.GetLogger "BlobTriggerFSharp"
        logger.LogInformation msg
