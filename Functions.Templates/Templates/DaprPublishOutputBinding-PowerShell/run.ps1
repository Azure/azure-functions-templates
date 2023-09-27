
using namespace System
using namespace Microsoft.Azure.WebJobs
using namespace Microsoft.Extensions.Logging
using namespace Microsoft.Azure.WebJobs.Extensions.Dapr
using namespace Newtonsoft.Json.Linq

# Sample Dapr Publish Output Binding
# See https://aka.ms/azure-function-dapr-publish-output-binding for more information about using this binding

# These tasks should be completed prior to running :
#      1. Install Dapr
# Run the app with below steps
#      1. Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 -- func host start
#      2. Function will be invoked by Timer trigger and publish messages to message bus.

param (
    $myTimer
)

Write-Host "PowerShell DaprPublish output binding function processed a request."

$currentTime = Get-Date

$publishOutputBindingReqBody = @{
    "payload" = "Invoked by Timer trigger: Hello, World! The time is $currentTime"
}

Push-OutputBinding -Name pubEvent -Value $publishOutputBindingReqBody