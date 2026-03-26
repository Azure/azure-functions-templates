# Input bindings are passed in via param block.
param($documents, $TriggerMetadata)

if ($documents.Count -gt 0) {
    Write-Host "Document Id: $($documents[0]._id)"
}
