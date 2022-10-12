<#
Sample SQL Input Binding
See https://aka.ms/sqlbindingsinput for more information about using this binding
These tasks should be completed prior to running:
    1. Update "commandText" in function.json - this should be the query to execute to retrieve the values being returned
    2. Add an app setting named "SqlConnectionString" containing the connection string to use for the SQL connection
    3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
@param Request The HttpRequest that triggered this function
@param items The array of objects returned by the SQL input binding
#>
using namespace System.Net

# Trigger and input binding data are passed in via the param block.
param($Request, $items)

# PowerShell function with SQL Input Binding processed a request
Write-Host "PowerShell SQL Binding function processed a request."

# Assign the value to return as the HTTP response. 
# The -Name value matches the name property in the function.json for the binding
Push-OutputBinding -Name Response -Value ([HttpResponseContext]@{
    StatusCode = [System.Net.HttpStatusCode]::OK
    Body = $items
})