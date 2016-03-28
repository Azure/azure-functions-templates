Param(
   [Parameter(Mandatory=$True)][string]$configFile,      
   [Parameter(Mandatory=$True)][string]$templatesFolderPath,
   [Parameter(Mandatory=$False)][string]$extZipPath
)


$HttpWaitTime = 2
$action = @{  "deploy" = "Deploy Template"
              "trigger" = "Execute Trigger"
              "checklog" = "Check Logs"   
                }

function GetConfiguration($configFile)
{
    Write-Host "Getting the Azure configuration"
    $config = Get-Content $configFile -Raw | ConvertFrom-Json
    $appName = $config.appName
    $userName = "`$$appName"
    $authInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $userName,$config.password)))    
    
    Write-Host -ForegroundColor Green "Complete"

    return @{ 
        appName = $appName
        authInfo = $authInfo
        scmEndpoint = "https://$appName.scm.azurewebsites.net"
        url = "https://$appName.azurewebsites.net"
    }
}

function GetLocalFile($path,$fileName)
{    
    if ($fileName)
    {
        $path = $path  + "\" + $fileName
    }

    if (!$path -or !(Test-Path -Path $path -PathType Leaf))
    {           
        Write-Host -ForegroundColor Red "Warning!: [FileNotFound] File $path does not exist"
        return
    }

    $file = Get-Content $path -Raw
    
    if ($path.ToLower().EndsWith(".json"))
    {
        $file = $file | ConvertFrom-Json
    }    
    return $file
}

function CreateResultFromException($template,$Exception,$action)
{    
    return @{
                Template = $template.Name
                Action = $action
                Status = "failed"
                TestLog = $Exception.Message
            }
}

# Create an array list with info (name, trigger, input and output binding) of all templates
function GetTemplates($templatesFolderPath)
{
    Write-Host "Fetching information for all templates"
    # Getting the list of template folders"
    $templatesList = Get-ChildItem $templatesFolderPath -Recurse | ?{ $_.PSIsContainer }

    $templates = New-Object System.Collections.ArrayList

    foreach ($template in $templatesList)
    { 
           
        $functionJson = GetLocalFile $template.FullName "function.json"
        
        if ($functionJson)
        {
            $trigger = $functionJson.bindings.Where({ if ($_) {($_.type).ToLower().EndsWith("trigger")}})
            $inputBinding = $functionJson.bindings.Where({ if ($_) {($_.direction).ToLower().EndsWith("in")}})        
            $outputBinding = $functionJson.bindings.Where({ if ($_) {($_.direction).ToLower().EndsWith("out")}})
        }

        $index = $templates.add(
            @{ 
                Name = $template.Name 
                Path = $template.FullName
                trigger = $trigger
                inputBinding = $inputBinding 
                outputBinding = $outputBinding                 
                inputData = GetLocalFile $template.FullName "sample.dat"
                TestOutput = GetLocalFile $template.FullName "TestOutput.json"
            })
    }

    Write-Host -ForegroundColor Green "Complete"
    return $templates
}

function UploadZip($config,$zipFilePath,$destinationPath)
{
    Write-Host -ForegroundColor Yellow -NoNewline "Uploading Zip file located at:$zipFilePath to $destinationPath"
    if (!$zipFilePath -or !(Test-Path -Path $zipFilePath -PathType Leaf))
    {
        throw [System.IO.FileNotFoundException] "File $zipFilePath does not exist"
    }
   
    $apiUrl = $config.scmEndpoint + "/api/zip/"
    if ($destinationPath)
    {
        $apiUrl = $apiUrl + $destinationPath
    }

    $response = Invoke-RestMethod -Uri $apiUrl -Headers @{Authorization=("Basic {0}" -f $config.authInfo)} -Method PUT -InFile $zipFilePath -ContentType "multipart/form-data"

    #todo Parse the response for failures
    if ($response)
    {
        throw "Zip upload failed"
    }
    Write-Host -ForegroundColor Green "...Complete"
}

function AddHostFeed($start)
{
    if ($start) { Write-Host }
    Write-Host "******************************************************************************************"
}

function CreateZipFile($template)
{
    $functionZipfile = $template.Name + ".zip"
    if (Test-Path -Path $functionZipfile -PathType Leaf) { Remove-Item $functionZipfile }

    $destinationPath = ".\$functionZipfile"
    Write-Host -ForegroundColor Yellow -NoNewline "Creating zip file to upload to kudu for" $template.Name
    Compress-Archive -Path $template.Path -DestinationPath $destinationPath
    Write-Host -ForegroundColor Green "...Complete"
    return $destinationPath    
}

function InvokeScmHttpDelete($config,$path)
{
    try
    { 
        Write-Host -ForegroundColor Yellow -NoNewline "Invoking delete for path:$path"
        $apiUrl = $config.scmEndpoint + "/api" + $path    
        $response = Invoke-RestMethod -Uri $apiUrl -Headers @{Authorization=("Basic {0}" -f $config.authInfo); "If-Match"="*"} -Method DELETE
        Write-Host -ForegroundColor Green "...Complete"
    } 
    Catch 
    {
        if (!($_.Exception.Message.Contains("404"))) { throw $_.Exception }
        Write-Host -ForegroundColor Green "...Complete"
    }
}

function RemoveTemplate($config, $template)
{
    # Deleting existing log files
    $path = "/vfs/site/wwwroot/"+ $template.Name +"/?recursive=true"
    InvokeScmHttpDelete $config $path
}

function DeployTemplate($config, $template)
{     
    try 
    {
        RemoveTemplate $config $template

        Start-Sleep -Seconds 2
        # Deleting existing function
        $path = "/vfs/LogFiles/Application/Functions/function/"+ $template.Name +"/?recursive=true" 
        InvokeScmHttpDelete $config $path

        # Zipping up the contents of the template for deployment
        $zipFilePath = CreateZipFile $template        

        # Upload the newly zipped file to remote location
        UploadZip $config $zipFilePath "site/wwwroot/"

        return CreateResult $template $action.deploy "passed" "Deployment Successful"
    }
    Catch
    {
        return CreateResultFromException $template $_.Exception $action.deploy
    }
}

function CreateResultFromResponse($template,$actualValue,$action)
{
    $name = $template.Name + "-" + $action
    $value = $null;
    if ($actualValue.contains($template.TestOutput.output))
    {
        return @{
                    Template = $template.Name
                    Action = $action                      
                    Status = "passed"
                    TestLog = "found matching value:" + $actualValue
                }
    }
    else 
    {
        return @{
                    Template = $template.Name
                    Action = $action                
                    Status = "failed"
                    TestLog = "Expected value:" + $template.TestOutput.output + "`n Actual value" + $actualValue
              }
    }
}

function CreateResult($template,$action,$status,$message)
{
    return @{
                Template = $template.Name
                Action = $action                            
                Status = $status
                TestLog = $message
            }
}

function ExecuteHttpTrigger($config,$template)
{
   try
   {
        $body = GetJsonWithoutWhiteSpace $template.inputData
        Write-Host -ForegroundColor Yellow -NoNewline "Executing trigger for "$template.Name
        Start-Sleep -s $HttpWaitTime
        $apiUrl = $config.url + "/api/" + $template.Name  + "?code=" + $config.functionSecret
        $response = Invoke-RestMethod -Uri $apiUrl -Method POST -Body $body -ContentType "application/json"
        Write-Host -ForegroundColor Green "...Complete"   
           
        if ($template.TestOutput.location.ToLower() -eq "httpResponse")
        {
            return CreateResultFromResponse $template $response $action.trigger
        }    
        return CreateResult $template "HttpTrigger Executed" "passed" $action.trigger
    }
    catch
    {
        Write-Host -ForegroundColor Green "...Complete"
        return CreateResultFromException $template $_.Exception $action.trigger
    }
}

function ExecuteTimerTrigger($config,$template)
{
   try
   {
        WakeUpHostProcess $config
        return CreateResult $template $action.trigger "passed" "Timer trigger will auto execute"
    }
    catch
    {
        Write-Host -ForegroundColor Green "...Complete"
        return CreateResultFromException $template $_.Exception $action.trigger
    }
}

function BooleanToResult($value)
{
    $result = "failed";
    if ($value) { $result = "passed"}  
    return $result
}

function GetJsonWithoutWhiteSpace($jsonData)
{
    # converts the file contents to custom object"
    $message = $jsonData | ConvertFrom-Json

    #covert it back to raw content but white space removed
    $message = $message | ConvertTo-Json -Compress
    return $message
}

function GetSha1($jsonContent,$functionSecret)
{
    [string]$key = $functionSecret
    [Byte[]]$keybyte = [Text.Encoding]::UTF8.GetBytes($key);
    $hmacsha = New-Object System.Security.Cryptography.HMACSHA1 -ArgumentList @(,$keybyte)

    [Byte[]]$messageByte = [Text.Encoding]::UTF8.GetBytes($jsonContent);

    [Byte[]]$sig = $hmacsha.ComputeHash($messageByte)
    return "sha1="+[BitConverter]::ToString($sig).Replace("-", [string].Empty);
}

function ExecuteWebHookTrigger($config,$template)
{
    try
    {
        $body = GetJsonWithoutWhiteSpace $template.inputData
        $sha1 = GetSha1 $body $config.functionSecret
    
        #add headers
        $headers = @{ "X-Hub-Signature"=$sha1; "X-GitHub-Event"='issue_comment'}    

        Write-Host -ForegroundColor Yellow -NoNewline "Executing trigger for " $template.Name
        Start-Sleep -s $HttpWaitTime
        $apiUrl = $config.url + "/api/" + $template.Name + "?code=" + $config.functionSecret
        $response = Invoke-RestMethod -Uri $apiUrl -Method POST -Body $body -ContentType "application/json" -Headers $headers
        Write-Host -ForegroundColor Green "...Complete"    
        [string]$response = ConvertTo-Json $response        
    
        if ($template.TestOutput.location.ToLower() -eq "httpResponse")
        {
            return CreateResultFromResponse $template $response $action.trigger
        }    

        return CreateResult $template "HttpTrigger Executed" "passed" $action.trigger
    } 
    catch 
    {
        Write-Host -ForegroundColor Green "...Complete"
        return CreateResultFromException $template $_.Exception $action.trigger
    }
    
}

function ExecuteTrigger($config,$template)
{

    $triggerType = $template.trigger.type
    if ($triggerType.ToLower() -eq "httptrigger" -and $template.trigger.webHookType)
    {
        $triggerType = $triggerType + "-" + $template.trigger.webHookType
    }

    switch ($triggerType)
    {
        "httptrigger" { return ExecuteHttpTrigger $config $template }
        "timerTrigger" { return ExecuteTimerTrigger $config $template }
        "httptrigger-genericJson" { return ExecuteWebHookTrigger $config $template }
        "httptrigger-github" { return ExecuteWebHookTrigger  $config $template }
        default { return $false }
    }
}

function GetLogFile($config,$template)
{
    try {
        $apiUrl = $config.scmEndpoint + "/api/vfs/LogFiles/Application/Functions/Function/" + $template.name + "/"
        $response = Invoke-RestMethod -Uri $apiUrl -Headers @{Authorization=("Basic {0}" -f $config.authInfo)} -Method GET    
        if ($response -and $response[0])
        {        
            $latestFile = $response[0]        
            foreach($file in $response)
            {
                if ([DateTime]($file.mtime) -ge [DateTime]($latestFile.mtime))
                {
                    $latestFile = $file
                }
            }

            $apiUrl = $apiUrl + $latestFile.Name
            $response = Invoke-RestMethod -Uri $apiUrl -Headers @{Authorization=("Basic {0}" -f $config.authInfo)} -Method GET       
            return $response
        }
    } 
    Catch 
    {
        if (!($_.Exception.Message.Contains("404"))) { throw $_.Exception }
    }
}

function GetFunctionSecret($config)
{   
    Write-Host -ForegroundColor Yellow -NoNewline "Getting function Secret"
    $keyFile = InvokeScmHttpGet $config "/vfs/data/functions/secrets/host.json"
    Write-Host -ForegroundColor Green "...Complete"
    return $keyFile.masterKey
}

function InvokeScmHttpGet($config,$path)
{
    $apiUrl = $config.scmEndpoint + "/api" + $path    
    $response = Invoke-RestMethod -Uri $apiUrl -Headers @{Authorization=("Basic {0}" -f $config.authInfo)} -Method GET
    return $response
}

function CreateResultFromLog($config,$template,$timeout)
{
    try 
    {
        Start-Sleep -s 3
        $i = 0
        While ($i -lt $timeout)
        {
            $response = GetLogFile $config $template
            if ($response -and $response.contains($template.TestOutput.output))
            { 
                Break 
            }
            Start-Sleep -Seconds 15
            $i = $i + 15
        }
    
        if ($response)
        {
            return CreateResultFromResponse $template $response $action.checklog
        } 
        else
        {
            return CreateResult $template $action.checklog "failed" "Log files not found"
        }

    }
    Catch
    {
        return CreateResultFromException $template $_.Exception $action.checklog
    }
}

function CreateHtml($testResult,$fileName)
{
    $Header = @"
    <script src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.11.3.min.js"></script>
    <script type='text/javascript'>
        `$(window).on('load', function () {
            cells = document.getElementsByTagName('td');

            for (var i = 0, len = cells.length; i < len; i++) {
                if (cells[i].innerHTML.toLocaleLowerCase() == 'passed') {
                    cells[i].style.backgroundColor = 'green';
                    cells[i].style.color = 'white';
                }
                if (cells[i].innerHTML.toLocaleLowerCase() == 'failed') {
                    cells[i].style.backgroundColor = 'red';
                    cells[i].style.color = 'white';
                }
				
				if (cells[i].innerHTML.toLocaleLowerCase() == 'skipped') {
                    cells[i].style.backgroundColor = 'orange';
                    cells[i].style.color = 'white';
                }
            }
        });

    </script>

    <center>
    <h1>Test Results<br></h1>
    </center>
    <style>
        TABLE {
            width: 1080px;
            margin-left: auto;
            margin-right: auto;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            border-collapse: collapse;
        }

    .green {
        background-color: green;
    }

    .red {
        background-color: red;
    }

        tr:hover {
            background-color: lightBlue;
            color: white;
        }

        TH {
            color: white;
            border-width: 1px;
            padding: 3px;
            border-style: solid;
            border-color: black;
            background-color: #6495ED;
        }

        TD {            
            border-width: 1px;
            padding: 3px;
            border-style: solid;
            border-color: black;
        }

        TD {
            border-width: 1px;
            padding: 3px;
            border-style: solid;
            border-color: black;
        }

        td:first-child {
            padding: 6px;
	        white-space: nowrap;
	    }

		td:nth-child(2) {
    	    padding: 6px;
	    	white-space: nowrap;
		}
	
		td:nth-child(3) {        
            padding: 6px;
	    	text-align: center;	
	    	white-space: nowrap;
		}
    </style>    
"@
    
    # Converting to Json will Expand the arraylist and Hastable Within it
    $jsonObject = $testResult | ConvertTo-Json

    # Converting to custom Object with all properties expanded
    $customObject = $jsonObject | ConvertFrom-Json 
        
    # Make data more presentable
    $formattedObject = $customObject | Select Template,Action,Status,@{Name="Log";Expression={$_.TestLog.Substring(0,[Math]::Min(($_.TestLog.Length),80)) + "..." }} 
    
    # Make data more presentable
    $formattedForHtml = $customObject | Select Template,Action,Status,@{Name="Log";Expression={$_.TestLog}}

    # Convert to HTML
    $html = $formattedForHtml | ConvertTo-Html -Head $Header

    # Create an HTML file
    $html | out-file ".\$fileName" -encoding ascii

    return $formattedObject
}

function WakeUpHostProcess($config)
{
    Write-Host -ForegroundColor Yellow -NoNewline "Waking up the host"
    $response = Invoke-RestMethod -Uri $config.url -Method GET
    Write-Host -ForegroundColor Green "...Complete"
}



function DeleteSiteExtension($config)
{
    # kill host processes allowing us to delete this site extension
    RestartSite $config

    Write-Host -ForegroundColor Yellow "Removing existing site extensions if any..."
    $path = "/vfs/SiteExtensions/?recursive=true"
    $response = InvokeScmHttpDelete $config $path
    Write-Host -ForegroundColor Green "Site Extension Removal Complete"
    WakeUpHostProcess $config
}

function RestartSite($config)
{
    try 
    {
        Write-Host -ForegroundColor Yellow "Restarting kudu site"
        # Get process info for kudu process
        $kuduProcess = InvokeScmHttpGet $config "/processes/0"    
    
        # Get list of all processes and kill non kudu w3wp process
        $allProcesses = InvokeScmHttpGet $config "/processes"

        foreach ($process in $allProcesses)
        {
            # if the process is not a kudu process kill it first
            if ($process.id -ne $kuduProcess.id -and $process.name -eq "w3wp")
            {
                #kill non scm process
                $path = "/processes/" + $process.id
                $response = InvokeScmHttpDelete $config $path
            }
        }

        # kill scm process now
        $response = InvokeScmHttpDelete $config "/processes/0"
    }
    Catch 
    {
        if (!($_.Exception.Message.Contains("502"))) { throw $_.Exception }
        Start-Sleep -Seconds 2
        
        # for the delete for kudu call that results in 502
        Write-Host -ForegroundColor Green "...Complete"

        Write-Host -ForegroundColor Green "Restart Complete"
    }
     
}

Try
{ 

    # Get the azure config
    $config = GetConfiguration $configFile

    # Get the secret key from the function            
    $secretJson = GetFunctionSecret $config
    $config.Add("functionSecret",$secretJson) 

    # Get metadata required to deploy templates
    $templates = GetTemplates $templatesFolderPath

    #Delete existing function site extension
    DeleteSiteExtension $config

    if ($extZipPath)
    {
        # Uploading site extention
        UploadZip $config $extZipPath

        # Restart site
        RestartSite $config

        # Wake up the host process to make sure the site is up
        WakeUpHostProcess $config
       
    }    

    # Collection to hold the execution result
    $testResult = New-Object System.Collections.ArrayList
    

    foreach ($template in $templates)
    {
        if ($template.TestOutput)
        {
            AddHostFeed

            # Deploy the template to azure
            $result = DeployTemplate $config $template         
            $index = $testResult.Add($result)

            # Execute Trigger
            $result = ExecuteTrigger $config $template
            $index = $testResult.Add($result)
            
            if ($template.TestOutPut.location.ToLower() -eq "logs")
            {                
                $result = CreateResultFromLog $config $template 120
                $index = $testResult.Add($result)
            }


            # Remove the template after it is done executing
            RemoveTemplate $config $template

            #$index = $testResult.Add($result)
            AddHostFeed $false
        }
    }

    $FormattedResult = CreateHtml $testResult "TestOut.html"
    
    $aggregateStatus = $true
    $FormattedResult.ForEach({$aggregateStatus =  (($_.Status -eq "passed") -and $aggregateStatus) })

    $FormattedResult
    if (!$aggregateStatus)
    {
        throw "Template Execution failed. Please check the results"
    }
    
}
Catch
{
    Write-Host $_.Exception.GetType().FullName, $_.Exception.Message
    throw $_.Exception
}
