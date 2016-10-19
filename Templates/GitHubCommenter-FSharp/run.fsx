// Please follow the link https://developer.github.com/v3/oauth/ to get information on GitHub authentication

#r "System.Net.Http"

open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Text
open FSharp.Interop.Dynamic

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

let Run(payload: obj, log: TraceWriter) =
    let comment = "{ \"body\": \"Thank you for your contribution, We will get to it shortly\" }";
    let label = "[ \"bug\" ]";

    if payload?action = "opened" then
        let issue = payload?issue
        if not (isNull issue) then
            log.Info(
                sprintf "%s posted an issue #%d: %s"
                    issue?user?login issue?number issue?title)

            SendGitHubRequest (issue?comments_url.ToString()) comment |> ignore
            SendGitHubRequest
                (sprintf "%s/labels" (issue?url.ToString())) label |> ignore

        let pr = payload?pull_request
        if not (isNull pr) then
            log.Info(sprintf "%s submitted pull request #%d: %s"
                pr?user?login pr?number pr?title)

            SendGitHubRequest (pr?comments_url.ToString()) comment |> ignore
