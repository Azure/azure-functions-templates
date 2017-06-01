#r "System.Net.Http"
#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"

open System
open System.Net
open System.Net.Http
open Microsoft.WindowsAzure.Storage.Table
open Newtonsoft.Json
open Newtonsoft.Json.Linq

type Person() =
    inherit TableEntity()
    member val Name: string = null with get, set

let Run(req: HttpRequestMessage, outTable: ICollector<Person>, log: TraceWriter) =
    async {
        let! data = req.Content.ReadAsStringAsync() |> Async.AwaitTask
        let json = JObject.Parse(data)
        let nameJ = json.["name"]

        if nameJ <> null then
            let name = nameJ.Value<string>()
            let person = Person()
            person.PartitionKey <- "Functions"
            person.RowKey <- Guid.NewGuid().ToString()
            person.Name <- name
            outTable.Add(person)

            return req.CreateResponse(HttpStatusCode.Created)
        else
            return req.CreateResponse(HttpStatusCode.BadRequest,
                "Please pass a name in the request body")
    } |> Async.StartAsTask
