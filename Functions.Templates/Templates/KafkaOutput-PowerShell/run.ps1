using namespace System.Net

# Input bindings are passed in via param block.
param($Request, $TriggerMetadata)

# Write to the Azure Functions log stream.
Write-Host "PowerShell HTTP trigger function processed a request."

# Interact with query parameters or the body of the request.
$message = $Request.Query.Message
if (-not $message) {
    $message = $Request.Body.Message
}

$body = "This HTTP triggered function executed successfully. Pass a message in the query string or in the request body for a personalized response."

if ($message) {
    $body = "Message received:  $message. The message transfered to the kafka broker."
}

Push-OutputBinding -Name Message -Value ("Message: " + $message)

# Associate values to output bindings by calling 'Push-OutputBinding'.
Push-OutputBinding -Name Response -Value ([HttpResponseContext]@{
    StatusCode = [HttpStatusCode]::OK
    Body = $body
})
