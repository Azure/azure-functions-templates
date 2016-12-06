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
    let people =
        query {
            for person in inTable do
            select person
        }
        |> Seq.map (fun person -> sprintf "\"Name\": \"%s\"" person.Name)
        |> String.concat ","

    req.CreateResponse(HttpStatusCode.OK, sprintf "{%s}" people)
