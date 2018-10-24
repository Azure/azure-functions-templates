
param([byte[]] $myBlob, $TriggerMetadata)

Write-Host "PowerShell Blob trigger function Processed blob! Name: $($TriggerMetadata.Name) Size: $($myBlob.Length) bytes"
