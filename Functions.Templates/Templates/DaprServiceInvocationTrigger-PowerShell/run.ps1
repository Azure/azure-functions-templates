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
#      1. Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 -- func host start
#      2. Invoke function app by dapr cli: dapr invoke --app-id functionapp --method {yourFunctionName}  --data '{ \"data\": {\"value\": { \"orderId\": \"41\" } } }'

param (
    $payload
)

Write-Host "Azure function triggered by Dapr Service Invocation Trigger."

$jsonString = $payload | ConvertTo-Json
Write-Host "Dapr service invocation trigger payload: $jsonString"