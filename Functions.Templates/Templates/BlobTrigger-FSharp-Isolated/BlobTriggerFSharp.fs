namespace Company.Function

open System
open System.IO
open Azure.Storage.Blobs;
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging

module BlobTriggerFSharp =

    [<Function("BlobTriggerFSharp")>]
    let run
        (
            [<BlobTrigger("PathValue/{name}", Connection = "ConnectionValue")>] client: BlobClient,
            name: string,
            context: FunctionContext
        ) =
        let logger
            = context.GetLogger "BlobTriggerFSharp"

        let text =
            client.DownloadContent().Value.Content.ToString()

        let msg =
            sprintf "F# Blob trigger function Processed blob\nName: %s \nData: %s" name text

        logger.LogInformation msg
