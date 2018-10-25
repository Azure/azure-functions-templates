# Input bindings are passed in via param block.
param([byte[]] $Blob, $TriggerMetadata)

# Write out the blob name and size to the information log.
Write-Host "PowerShell Blob trigger function Processed blob! Name: $($TriggerMetadata.Name) Size: $($Blob.Length) bytes"
