$requestBody = Get-Content $req -Raw | ConvertFrom-Json
$name = $requestBody.name

if ($req_query_name) 
{
    $name = $req_query_name 
}

Out-File -Encoding Ascii $res -inputObject "Hello $name"