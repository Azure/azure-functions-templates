$isPr = Test-Path env:APPVEYOR_PULL_REQUEST_NUMBER
$directoryPath = Split-Path $MyInvocation.MyCommand.Path -Parent

if (-not $isPr) {
  Mkdir "$directoryPath/../buildoutput"
  Compress-Archive ../Functions.Templates/bin/VS/Release/* $directoryPath\..\buildoutput\tosign.zip


  if ($env:SkipAssemblySigning -eq "true") {
    "Assembly signing disabled. Skipping signing process."
    exit 0;
  }

  $ctx = New-AzureStorageContext $env:FILES_ACCOUNT_NAME $env:FILES_ACCOUNT_KEY
  Set-AzureStorageBlobContent "$directoryPath/../buildoutput/tosign.zip" "azure-webjobs-extensions" -Blob "$env:APPVEYOR_BUILD_VERSION.zip" -Context $ctx

  $queue = Get-AzureStorageQueue "signing-jobs" -Context $ctx

  $messageBody = "SignNupkgs;azure-webjobs-extensions;$env:APPVEYOR_BUILD_VERSION.zip"
  $queue.CloudQueue.AddMessage($messageBody)
}