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
            [<BlobTrigger("PathValue/{name}", Connection = "ConnectionValue")>] client: Stream,
            name: string,
            context: FunctionContext
        ) =
        let logger
            = context.GetLogger "BlobTriggerFSharp"

        use blobStreamReader
            = new StreamReader(stream)

        let blobContent
            = blobStreamReader.ReadToEndAsync() |> Async.AwaitTask

        let msg =
            sprintf "Blob content:\nContent: %s" blobContent

        logger.LogInformation msg
