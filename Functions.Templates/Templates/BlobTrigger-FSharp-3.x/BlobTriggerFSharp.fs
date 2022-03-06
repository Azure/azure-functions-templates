namespace Company.Function

open System.IO
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Host
open Microsoft.Extensions.Logging

module BlobTriggerFSharp =

    [<FunctionName("BlobTriggerFSharp")>]
    let run ([<BlobTrigger("PathValue/{name}", Connection = "ConnectionValue")>] myBlob: Stream, name: string, log: ILogger) =
        let msg = sprintf "F# Blob trigger function Processed blob\nName: %s \n Size: %d Bytes" name myBlob.Length
        log.LogInformation msg
