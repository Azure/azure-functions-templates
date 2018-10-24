# Input bindings are passed in via param block.
param([byte[]] $myBlob, $TriggerMetadata)

# Write out the blob name and size to the information log.
Write-Host "PowerShell Blob trigger function Processed blob! Name: $($TriggerMetadata.Name) Size: $($myBlob.Length) bytes"
