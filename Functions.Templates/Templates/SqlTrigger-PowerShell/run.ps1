<#
Sample SQL Trigger Binding
See https://aka.ms/sqltrigger for more information about using this binding
These tasks should be completed prior to running:
    1. Update "tableName" in function.json - this should be the table that is monitored for changes and triggers/invokes the function.
    2. Add an app setting named "SqlConnectionString" containing the connection string to use for the SQL connection
    3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
@param Request The HttpRequest that triggered this function
@param items The array of objects returned by the SQL input binding
#>
using namespace System.Net

# Trigger and input binding data are passed in via the param block.
param($changes)

# Write to the Azure Functions log stream.
Write-Host "PowerShell function with SQL Trigger Binding processed a request."

# The output is used to inspect the trigger binding parameter in test methods.
# Use -Compress to remove new lines and spaces for testing purposes.
$changesJson = $changes | ConvertTo-Json -Compress
Write-Host "SQL Changes: $changesJson"