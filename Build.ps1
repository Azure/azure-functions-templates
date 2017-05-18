param (    
    [ValidateSet('Portal', 'VS', 'All')]
    [System.String]$target,    
    [System.String]$templateVersion
)

Add-Type -AssemblyName System.IO.Compression.FileSystem

function LogErrorAndExit($errorMessage, $exception) {    
    Write-Host $errorMessage -ForegroundColor Yellow        
    if ($exception -ne $null) {
        Write-Host "Error occured at line:" + $exception.InvocationInfo.ScriptLineNumber -ForegroundColor Yellow
        Write-Host $exception.Message -ForegroundColor Red
    }    
    Exit
}

function LogSuccess($message) {
    Write-Host $message -ForegroundColor Green    
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
    $binDirectory = Join-Path $rootPath -ChildPath "bin"
    $templatesPath = Join-Path $rootPath -ChildPath "\Templates\"    
       
    Enum TargetHost {
        Portal
        VS
        All
    }

    # Start with a clean slate
    if (Test-Path $binDirectory) {
        Remove-Item $binDirectory -Recurse -Confirm:$false    
    }

    $toolsDir = Join-Path $binDirectory -ChildPath "\Tools\"
    New-Item -ItemType Directory $toolsDir

    # Download dotnet CLI
    $dotnetCliDownloadUrl = "https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/2.0.0/dotnet-dev-win-x86.latest.zip"
    $dotnetCliZip = Join-Path $toolsDir -ChildPath "dotnet.zip"
    Download $dotnetCliDownloadUrl $dotnetCliZip    

    # Unzip dotnet CLI
    $cliDir = Join-Path $toolsDir -ChildPath "\cli\"
    New-Item -ItemType Directory $cliDir
    Unzip $dotnetCliZip $cliDir    
        
    # Download code formatter tool
    $codeFormatterZip = Join-Path $toolsDir -ChildPath "codeFormatter.zip"
    $codeFormatterDownloadUrl = "https://github.com/dotnet/codeformatter/releases/download/v1.0.0-alpha6/CodeFormatter.zip"    
    Download $codeFormatterDownloadUrl $codeFormatterZip    

    # Unzip code formatter tool    
    Unzip $codeFormatterZip $toolsDir
    
    $dotnetExe = Join-Path $binDirectory -ChildPath "\tools\cli\dotnet.exe"    
    $codeFormatterExe = Join-Path $binDirectory -ChildPath "tools\CodeFormatter\CodeFormatter.exe"

    # creating a directory to hold nuget packages
    $nugetPackageDir = Join-Path $binDirectory -ChildPath "nupkg"
    New-Item $nugetPackageDir -ItemType Directory

    # Install generic templates
    Invoke-Expression -Command "$dotnetExe new -i $templatesPath"
    if ($LastExitCode -ne 0) {
        LogErrorAndExit "Failed to install templates"
    }

    if ($templateVersion -eq [String]::Empty)
    {
        $templateVersion = "1.0.0"
    }

    # Build templates for portal
    if ($target -eq [TargetHost]::All.ToString() -or $target -eq [TargetHost]::Portal.ToString()) {        

        $portalDirectory = Join-Path $binDirectory -ChildPath "portal"
        New-Item $portalDirectory -ItemType Directory

        $portalSourceDirectory = Join-Path $portalDirectory -ChildPath "src"
        New-Item $portalSourceDirectory -ItemType Directory
        Set-Location $portalSourceDirectory
        
        # Keep only portal specific content
        Invoke-Expression -Command "$dotnetExe new functions --portalTemplates true --vsTemplates false"
        if ($LastExitCode -ne 0) {
            LogErrorAndExit "Failed to execute templates"
        }        

        # re-indent the .cs files
        $codeFormatterPortalProj = Join-Path $portalSourceDirectory -ChildPath "codeFormat.csproj"
        Invoke-Expression -Command "$codeFormatterExe $codeFormatterPortalProj /nocopyright"

        # renaming run.cs to run.csx for portal
        Get-ChildItem -Recurse -Include *.cs | Rename-Item -NewName { $_.Name.replace("run.cs","run.csx") }

        # Generate a nuget package for portal templates
        $portalNuspec = Join-Path $portalSourceDirectory -ChildPath "PortalTemplates.nuspec"
        nuget.exe pack $portalNuspec -Version $templateVersion -OutputDirectory $NugetPackageDir
        if ($LastExitCode -ne 0) {
            LogErrorAndExit "Error creating nuget package"
        }

        # Create a release directory for extraction
        $portalReleaseDir = Join-Path $portalDirectory -ChildPath "release"
        New-Item $portalReleaseDir -ItemType Directory
        
        $packageName = "Azure.Functions.Templates.Portal"
        Set-Location $portalReleaseDir
                
        # Extracting nuget content for portal to consume
        nuget.exe install $packageName -Version $templateVersion -Source $nugetPackageDir -OutputDirectory $portalReleaseDir -ExcludeVersion 
        if ($LastExitCode -ne 0) {
            LogErrorAndExit "Could not install Azure.Functions.Templates.Portal nuget package"
        }
    }

    if ($target -eq [TargetHost]::All.ToString() -or $target -eq [TargetHost]::VS.ToString()) {        
        $vsDirectory = Join-Path $binDirectory -ChildPath "VS"
        New-Item $vsDirectory -ItemType Directory

        $vsSourceDirectory = Join-Path $vsDirectory -ChildPath "src"
        New-Item $vsSourceDirectory -ItemType Directory
        Set-Location $vsSourceDirectory
        
        # Keep only vs specific content
        Invoke-Expression -Command "$dotnetExe new functions --portalTemplates false --vsTemplates true"
        if ($LastExitCode -ne 0) {
            LogErrorAndExit "Failed to execute templates"
        }        

        # re-indent the .cs files
        $codeFormattervsProj = Join-Path $vsSourceDirectory -ChildPath "codeFormat.csproj"
        Invoke-Expression -Command "$codeFormatterExe $codeFormattervsProj /nocopyright"        

        # Generate a nuget package for portal templates
        $vsNuspec = Join-Path $vsSourceDirectory -ChildPath "ItemTemplates.nuspec"
        Write-Host "nuget.exe pack $vsNuspec -Version $templateVersion -OutputDirectory $NugetPackageDir"
        nuget.exe pack $vsNuspec -Version $templateVersion -OutputDirectory $NugetPackageDir
        if ($LastExitCode -ne 0) {
            LogErrorAndExit "Error creating nuget package"
        }

        $projectTemplateBuildFile = Join-Path $rootPath -ChildPath "ProjectTemplate\Template.proj"
        msbuild $projectTemplateBuildFile /t:Clean;
        msbuild $projectTemplateBuildFile /p:PackageVersion=$templateVersion

        $projectTemplateNuget = Join-Path $rootPath -ChildPath "\ProjectTemplate\bin\*.nupkg"
        Copy-Item $projectTemplateNuget $nugetPackageDir
    }    
}
catch {
    LogErrorAndExit "UnKnown Error" $_.Exception
}