#r "System.Net.Http"
#r "Microsoft.WindowsAzure.Storage"

open System.Linq
open System.Net
open System.Net.Http
open Microsoft.WindowsAzure.Storage.Table

type Person() =
    inherit TableEntity()
    member val Name: string = null with get, set

let Run(req: HttpRequestMessage, inTable: IQueryable<Person>, log: TraceWriter) =
    query {
        for person in inTable do
        select person
    } |> Seq.iter (fun person ->
        log.Info(sprintf "Name: %s" person.Name))

    try
        req.CreateResponse(HttpStatusCode.OK, inTable.ToList())
    with _ ->
        req.CreateResponse(HttpStatusCode.OK)
