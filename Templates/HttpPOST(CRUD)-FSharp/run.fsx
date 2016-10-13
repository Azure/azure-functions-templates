#r "System.Net.Http"
#r "Microsoft.WindowsAzure.Storage"

open System
open System.Net
open System.Net.Http
open Microsoft.WindowsAzure.Storage.Table
open FSharp.Interop.Dynamic

type Person() =
    inherit TableEntity()
    member val Name: string = null with get, set

let Run(req: HttpRequestMessage, outTable: ICollector<Person>, log: TraceWriter) =
    async {
        let! data = req.Content.ReadAsAsync<obj>() |> Async.AwaitTask
        let name = data?name

        if isNull name then
            return req.CreateResponse(HttpStatusCode.BadRequest,
                "Please pass a name in the request body")
        else
            let person = Person()
            person.PartitionKey <- "Functions"
            person.RowKey <- Guid.NewGuid().ToString()
            person.Name <- name
            outTable.Add(person)

            return req.CreateResponse(HttpStatusCode.Created)
    } |> Async.StartAsTask
