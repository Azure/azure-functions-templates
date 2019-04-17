using namespace System.Net

param($Request, $TriggerMetadata, $ConnectionInfo)

Push-OutputBinding -Name Response -Value ([HttpResponseContext]@{
    StatusCode = [HttpStatusCode]::OK
    ContentType = "application/json"
    Body = $ConnectionInfo
})