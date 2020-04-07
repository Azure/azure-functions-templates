param($Context)

$output = @()

$output += Invoke-ActivityFunction -FunctionName 'Hello' -Input 'Tokyo'
$output += Invoke-ActivityFunction -FunctionName 'Hello' -Input 'Seattle'
$output += Invoke-ActivityFunction -FunctionName 'Hello' -Input 'London'

$output
