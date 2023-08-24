using namespace System
using namespace Microsoft.Azure.WebJobs
using namespace Microsoft.Extensions.Logging
using namespace Microsoft.Azure.WebJobs.Extensions.Dapr
using namespace Newtonsoft.Json.Linq

# Sample Dapr Binding Trigger
# See https://aka.ms/azure-functions-dapr for more information about using this binding

# These tasks should be completed prior to running :
#      1. Install Dapr
#      2. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
# Run the app with below steps
#      1. Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 --resources-path .\components\ -- func host start
#      2. Invoke function app: dapr invoke --app-id functionapp --method {functionName}

param (
    $payload, $secret
)

# PowerShell function processed a CreateNewOrder request from the Dapr Runtime.
Write-Host "PowerShell ServiceInvocation trigger with DaprSecret input binding function processed a request."

# Convert the object to a JSON-formatted string with ConvertTo-Json
$jsonString = $secret | ConvertTo-Json

Write-Host "$jsonString"