namespace Company.Function

open System
open System.IO
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging

module BlobTriggerFSharp =

    [<Function("BlobTriggerFSharp")>]
    let run
        (
            [<BlobTrigger("PathValue/{name}", Connection = "ConnectionValue")>] myBlob: Stream,
            name: string,
            context: FunctionContext
        ) =
        let logger
            = context.GetLogger "BlobTriggerFSharp"

        use blobStreamReader
            = new StreamReader(myBlob)

        let blobContent
            = blobStreamReader.ReadToEndAsync() |> Async.AwaitTask

        let msg =
            sprintf "F# Blob trigger function Processed blob\nName: %s \n Data: %s" name blobContent

        logger.LogInformation msg
