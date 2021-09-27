param($Context)

$output = @()

$output += Invoke-DurableActivity -FunctionName 'Hello' -Input 'Tokyo'
$output += Invoke-DurableActivity -FunctionName 'Hello' -Input 'Seattle'
$output += Invoke-DurableActivity -FunctionName 'Hello' -Input 'London'

$output
