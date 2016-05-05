$name = Get-Content $Env:req

if ($Env:req_query_name) 
{
    $name = $Env:req_query_name 
}

Out-File -Encoding Ascii $Env:res -inputObject "Hello $name"