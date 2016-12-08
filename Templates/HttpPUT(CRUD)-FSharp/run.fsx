#r "System.Net.Http"
#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"

open System
open System.Net
open System.Net.Http
open Microsoft.WindowsAzure.Storage.Table
open Newtonsoft.Json
open Newtonsoft.Json.Linq

type Person(name, partitionKey, rowKey) =
    inherit TableEntity(partitionKey, rowKey)
    member val Name: string = name with get, set

let Run(req: HttpRequestMessage, outTable: CloudTable, log: TraceWriter) =
    async {
        let! data = req.Content.ReadAsStringAsync() |> Async.AwaitTask
        let json = JObject.Parse(data)
        let nameJ = json.["name"]
        let partitionKeyJ = json.["partitionKey"]
        let rowKeyJ = json.["rowKey"]

        if nameJ = null || partitionKeyJ = null || rowKeyJ = null then
            let response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            response.Content <-
                new StringContent("A non-empty name, partitionKey, and rowKey must be specified.")
            return response
        else
            let person =
                Person(
                    nameJ.Value<string>(),
                    partitionKeyJ.Value<string>(),
                    rowKeyJ.Value<string>())
            log.Info(sprintf "PersonName=%s" person.Name)
            let updateOperation = TableOperation.InsertOrReplace(person)
            let result = outTable.Execute(updateOperation)
            return new HttpResponseMessage(enum<HttpStatusCode>(result.HttpStatusCode))
    } |> Async.RunSynchronously
