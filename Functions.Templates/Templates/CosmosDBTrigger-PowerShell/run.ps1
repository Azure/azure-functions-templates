# This template uses an outdated version of the Azure Cosmos DB extension. Learn about migrating to the new extension at https://aka.ms/migrate-to-cosmos-extension-v4

# Input bindings are passed in via param block.
param($Documents, $TriggerMetadata)

if ($Documents.Count -gt 0) {
    Write-Host "Document Id: $($Documents[0].id)"
}
