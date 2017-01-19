#r "System.Net.Http"
#r "Microsoft.Azure.ApiHub.Sdk" 

open System.Net
open System.Net.Http
open Microsoft.Azure.ApiHub

type Contact() =
    member val Id: string = null with get, set
    member val FirstName: string = null with get, set
    member val LastName: string = null with get, set

let Run(req: HttpRequestMessage, input: ITable<Contact>, log: TraceWriter) =
    async {
        log.Info(sprintf 
            "F# HTTP trigger function processed a request.")

        let rec q (cont: ContinuationToken) = async {
            let! segment =
                input.ListEntitiesAsync(continuationToken = cont)
                |> Async.AwaitTask
            
            for item in segment.Items do
                log.Info(sprintf "%s %s" item.FirstName item.LastName)

            match segment.ContinuationToken with
            | null -> return ()
            | token -> q token |> Async.RunSynchronously
        }

        q null |> Async.RunSynchronously
        return req.CreateResponse(HttpStatusCode.OK)
    } |> Async.StartAsTask
