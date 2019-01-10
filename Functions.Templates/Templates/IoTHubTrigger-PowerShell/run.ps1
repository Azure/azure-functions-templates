param($IoTHubMessages, $TriggerMetadata)

Write-Host "PowerShell eventhub trigger function called for message array: $IoTHubMessages"

$IoTHubMessages | ForEach-Object { Write-Host "Processed message: $_" }
