# Input bindings are passed in via param block.
param([string] $MyQueueItem, $TriggerMetadata)

# Write out the queue message and insertion time to the information log.
Write-Host "PowerShell rabbitmq trigger function processed work item: $MyQueueItem"
Write-Host "Queue item insertion time: $($TriggerMetadata.InsertionTime)"
