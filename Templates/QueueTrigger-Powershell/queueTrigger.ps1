$in = Get-Content $Env:input
$output = $Env:output

[Console]::WriteLine("Powershell script processed queue message '$in'")
$entity = [string]::Format('{{ "timestamp": "{0}", "title": "queue message: {1}" }}', $(get-date).ToString(), $in)
$entity | Out-File -Encoding Ascii $output