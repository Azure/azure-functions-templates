#r "System.Net.Http"
#r "Microsoft.WindowsAzure.Storage"

open System
open System.Net
open System.Net.Http
open Microsoft.WindowsAzure.Storage.Table

type Person() =
    inherit TableEntity()
    member val Name: string = null with get, set

let Run(person: Person, outTable: CloudTable, log: TraceWriter) =
    if String.IsNullOrEmpty(person.Name) then
        let response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        response.Content <-
            new StringContent("A non-empty Name must be specified.")
        response
    else
        log.Info(sprintf "PersonName=%s" person.Name)
        let updateOperation = TableOperation.InsertOrReplace(person)
        let result = outTable.Execute(updateOperation)
        new HttpResponseMessage(enum<HttpStatusCode>(result.HttpStatusCode))
