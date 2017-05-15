#r "System.Net.Http"
#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"

open System
open System.Net
open System.Net.Http
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Queue

type Person() =
    member val id: Guid = Guid.NewGuid() with get    
    member val firstName: string = null with get, set    
    member val lastName: string = null with get, set    
    member val address: string = null with get, set    

let inline getValue (value:JToken) = 
    match isNull value with
    | false -> value.Value<string>()  
    | true -> ""

let hydrate data = 
    let json = JObject.Parse(data)
    let person = Person()
    person.firstName <- getValue json.["firstName"]
    person.lastName <- getValue json.["lastName"] 
    person.address <- getValue json.["address"] 
    person

let isValid (person:Person) =
    [person.firstName; person.lastName; person.address]
    |> List.forall (not << String.IsNullOrEmpty)

let Run(req: HttpRequestMessage, personItem:string byref) =
    let data = req.Content.ReadAsStringAsync().Result
    let person = hydrate data
    match isValid person with
    | true -> 
        let queueMessage = person |> JsonConvert.SerializeObject
        personItem <- queueMessage
        req.CreateResponse(HttpStatusCode.Created)
    | false ->
        req.CreateResponse(HttpStatusCode.BadRequest, "Please pass all of the values in the request body")
