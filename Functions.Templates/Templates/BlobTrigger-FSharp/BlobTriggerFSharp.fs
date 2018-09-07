namespace Company.Function

open System.IO
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Host

module BlobTriggerFSharp =

    [<FunctionName("BlobTriggerFSharp")>]
    let run ([<BlobTrigger("PathValue/{name}", Connection = "ConnectionValue")>] myBlob: Stream, name: string, log: TraceWriter) =
        let msg = sprintf "F# Blob trigger function Processed blob\nName: %s \n Size: %d Bytes" name myBlob.Length
        log.Info msg
