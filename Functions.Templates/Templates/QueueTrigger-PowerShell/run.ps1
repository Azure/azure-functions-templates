param([string] $myQueueItem, $TriggerMetadata)

Write-Host "PowerShell queue trigger function processed work item: $myQueueItem"
Write-Host "Queue item insertion time: $($TriggerMetadata.InsertionTime)"
