$ProgressPreference = "SilentlyContinue"
Add-Type -AssemblyName System.IO.Compression.FileSystem

function LogErrorAndExit($errorMessage, $exception) {    
    Write-Output $errorMessage
    if ($exception -ne $null) {        
        Write-Output $exception|format-list -force
    }    
    Exit
}

function LogSuccess($message) {
    Write-Output $message
}

function Download([string]$url, [string]$outputFilePath) {        
    try {
        Invoke-WebRequest -Uri $url -OutFile $outputFilePath 
        LogSuccess "Download complete for $url"
    } catch {        
        LogErrorAndExit "Download failed for $url" $_.Exception
    }   
}

function Unzip([string]$zipfilePath, [string]$outputpath) {    
    try {
        [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfilePath, $outputpath)        
        LogSuccess "Unzipped:$zipfilePath"
    }
    catch {
        LogErrorAndExit "Unzip failed for:$zipfilePath" $_.Exception
    }
}

# Main Code Block
try {       
    $rootPath = Get-Location

    $toolsDir = Join-Path $rootPath -ChildPath "\Tools\"      

    # Start with a clean slate
    if (-Not (Test-Path $toolsDir)) {
        New-Item -ItemType Directory $toolsDir
    }    

    # Check if cli is present
    $cliDir = Join-Path $toolsDir -ChildPath "\cli\"
    if (-Not( Test-Path $cliDir)) {
        # Download dotnet CLI
        $dotnetCliDownloadUrl = "https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/2.0.0/dotnet-dev-win-x86.latest.zip"
        $dotnetCliZip = Join-Path $toolsDir -ChildPath "dotnet.zip"
        Download $dotnetCliDownloadUrl $dotnetCliZip    

        # Unzip dotnet CLI    
        New-Item -ItemType Directory $cliDir
        Unzip $dotnetCliZip $cliDir    
    }    

    # Check if code formatter is present
    $codeFormatterPath = Join-Path $toolsDir -ChildPath "\codeFormatter\"    
    if (-Not(Test-Path $codeFormatterPath)) {        
        # Download code formatter tool
        $codeFormatterZip = Join-Path $toolsDir -ChildPath "codeFormatter.zip"
        $codeFormatterDownloadUrl = "https://github.com/dotnet/codeformatter/releases/download/v1.0.0-alpha6/CodeFormatter.zip"    
        Download $codeFormatterDownloadUrl $codeFormatterZip    

        # Unzip code formatter tool    
        Unzip $codeFormatterZip $toolsDir      
    }

    # Check if createTemplateConfigPath is present
    $createTemplateConfigPath = Join-Path $toolsDir -ChildPath "\CreateTemplateConfig\"    
    if (-Not(Test-Path $createTemplateConfigPath)) {        
        # Download code formatter tool
        $createTemplateConfigZip = Join-Path $toolsDir -ChildPath "CreateTemplateConfig.zip"
        $createTemplateConfigDownloadUrl = "https://github.com/Azure/azure-webjobs-sdk-templates/releases/download/1/CreateTemplateConfig.zip"    
        Download $createTemplateConfigDownloadUrl $createTemplateConfigZip    

        # Unzip code formatter tool        
        Unzip $createTemplateConfigZip $toolsDir        
    }    
    
    # Check if the nugetExe is present
    $nugetExe = Join-Path $toolsDir -ChildPath "nuget.exe"
    if (-Not(Test-Path $nugetExe)) {
        # downloading nuget.exe
        $nugetUrl = "https://dist.nuget.org/win-x86-commandline/v4.1.0/nuget.exe"
        Download $nugetUrl "$toolsDir\nuget.exe"
    }
}
catch {
    LogErrorAndExit "UnKnown Error" $_.Exception
}