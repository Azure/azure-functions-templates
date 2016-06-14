Function Test-BlobTrigger-CSharp($config, $templates)
{
    # Arrange
    $template = GetTemplate $templates "BlobTrigger-CSharp"    
    $blobContent = "sample blob data"
    $blobName = "myBlob.txt"
    $cleanStart = $True    
    $testResult = @{
        Title = $template.Name
        Log = ""
        Outcome = "Passed"
    }

    # DeployTemplate
    $result = DeployTemplate $config $template
    $testResult.Log += ($result.log + "`n")
    if (-Not $result.success)
    {
        $testResult.Outcome = "Failed"
        return $testResult
    } 

    # Execute Trigger
    $containerName = ExtractContainerName $template.trigger.path
    $result = ExecuteBlobTrigger $config $containerName $blobName $blobContent $cleanStart
    $testResult.Log += ($result.log + "`n")
    
    if (-Not $result.success)
    {
        $testResult.Outcome = "Failed"
        return $testResult
    }    
    
    # CheckLogs
    $result = CheckLogs $config $template $blobContent 120
    $testResult.Log += ("Log File from functions`n" +  $result.log + "`n")
    if (-Not $result.success)
    {
        $testResult.Outcome = "Failed"
        return $testResult
    }    

    # Clean up    
    RemoveTemplate $config $template    

    return $testResult
}