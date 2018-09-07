# Input bindings are passed in via param block.
param($req, $TriggerMetadata)

# Write to the Azure Functions log streams as you would in a normal PowerShell script.
Write-Verbose "PowerShell HTTP trigger function processed a request." -Verbose

# Interact with query parameters, the body of the request, etc.
$name = $req.Query.Name
if (-not $name) { $name = $req.Body.Name }

if($name) {
    $status = 200
    $body = "Hello " + $name
}
else {
    $status = 400
    $body = "Please pass a name on the query string or in the request body."
}

# Associate values to output bindings by calling 'Push-OutputBinding'.
Push-OutputBinding -Name res -Value ([HttpResponseContext]@{
    StatusCode = $status
    Body = $body
})
