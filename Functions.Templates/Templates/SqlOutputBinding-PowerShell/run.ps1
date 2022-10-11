<#
Sample SQL Output Binding
See https://aka.ms/sqlbindingsinput for more information about using this binding
These tasks should be completed prior to running :
    1. Update "commandText" in function.json - this should be the name of the table that you wish to upsert values to
    2. Add an app setting named "SqlConnectionString" containing the connection string to use for the SQL connection
    3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[3.*, 4.0.0)"
@param Request The HttpRequest that triggered this function
#>
using namespace System.Net

# Trigger binding data passed in via param block
param($Request)

# Write to the Azure Functions log stream.
Write-Host "PowerShell SQL Binding function processed a request."

# Set results array to req_body for upsertion
# Note that this expects the body to be a JSON object or array of objects which has a property 
# matching each of the columns in the table to upsert to.
$req_body = $Request.Body | ConvertTo-Json

# Associate values to output bindings by calling 'Push-OutputBinding'.
Push-OutputBinding -Name Response -Value ([HttpResponseContext]@{
    StatusCode = [HttpStatusCode]::OK
    Body = $req_body
    ContentType = "application/json"
})