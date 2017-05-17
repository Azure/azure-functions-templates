// Please follow the link https://developer.github.com/v3/oauth/ to get information on GitHub authentication

#r "System.Net.Http"
#r "Newtonsoft.Json"

open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Text
open Newtonsoft.Json
open Newtonsoft.Json.Linq

let SendGitHubRequest (url: string) requestBody =
    async {
        use client = new HttpClient()

        client.DefaultRequestHeaders.UserAgent.Add(
            ProductInfoHeaderValue("username", "version"))

        // Add the GITHUB_CREDENTIALS as an app setting, Value for the app setting is a base64 encoded string in the following format
        // "Username:Password" or "Username:PersonalAccessToken"
        // Please follow the link https://developer.github.com/v3/oauth/ to get more information on GitHub authentication 
        client.DefaultRequestHeaders.Authorization <-
            AuthenticationHeaderValue("Basic",
                Environment.GetEnvironmentVariable("GITHUB_CREDENTIALS"))
        use content =
            new StringContent(requestBody, Encoding.UTF8, "application/json")
        return! client.PostAsync(url, content) |> Async.AwaitTask
    } |> Async.RunSynchronously

let rec hasProp (key: string list) (from: JObject) =
    match from with
    | null -> false
    | _ ->
        let x = from.[key.Head]
        match x with
        | null -> false
        | _ ->
            match key with
            | [_] -> true
            | _::tl -> hasProp tl (x.Value<JObject>())
            | [] -> false

let rec prop<'T> (key: string list) (def: 'T) (from: JObject) =
    match from with
    | null -> def
    | _ ->
        let x = from.[key.Head]
        match x with
        | null -> def
        | _ ->
            match key with
            | [_] -> x.Value<'T>()
            | _::tl ->
                prop<'T> tl def (x.Value<JObject>())
            | [] -> def

let Run(payload: string, log: TraceWriter) =
    let comment = "{ \"body\": \"Thank you for your contribution, We will get to it shortly\" }";
    let label = "[ \"bug\" ]";

    let json = JObject.Parse(payload)
    if json |> prop ["action"] "none" = "opened" then
        if hasProp ["issue"] json then
            log.Info(
                sprintf "%s posted an issue #%d: %s"
                    (prop ["issue"; "user"; "login"] "unknown user" json)
                    (prop ["issue"; "number"] 0 json)
                    (prop ["issue"; "title"] "unknown title" json)
                    )
            SendGitHubRequest (prop ["issue"; "comments_url"] "" json) comment
                |> ignore
            SendGitHubRequest
                (sprintf "%s/labels" (prop ["issue"; "url"] "" json)) label
                |> ignore

        if hasProp ["pull_request"] json then
            log.Info(
                sprintf "%s submitted pull request #%d: %s"
                    (prop ["pull_request"; "user"; "login"] "unknown user" json)
                    (prop ["pull_request"; "number"] 0 json)
                    (prop ["pull_request"; "title"] "unknown title" json)
                    )
            SendGitHubRequest
                (prop ["pull_request"; "comments_url"] "" json) comment
                |> ignore
