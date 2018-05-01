param(
    [Parameter(Mandatory = $false)][bool]$runningInAppveyorEnv
)

$ProgressPreference = "SilentlyContinue"
Add-Type -AssemblyName System.IO.Compression.FileSystem
$Currentlocation = (Get-Location).path

function LogErrorAndExit($errorMessage, $exception) {
    Write-Output $errorMessage
    if ($exception -ne $null) {
        Write-Output $exception|format-list -force
    }    
    Exit 1
}

function LogSuccess($message) {
    Write-Output $message
}

function LogAppVeyorMessage($message, [bool]$runningInAppveyorEnv) {
    if ($runningInAppveyorEnv) {
        Add-AppveyorMessage $message -Category Information
    }
}

function LogAppVeyorErrorMessage($message, [bool]$runningInAppveyorEnv) {
    if ($runningInAppveyorEnv) {
        Add-AppveyorMessage $message -Category Error
    }
}

function Download([string]$url, [string]$outputFilePath) {
    try {
        Invoke-WebRequest -Uri $url -OutFile $outputFilePath 
        LogSuccess "Download complete for $url"
    }
    catch {
        LogErrorAndExit "Download failed for $url" $_.Exception
    }   
}

function dotnetBuild($csprojPath, $fallBackFolderPath) {
    Write-Host "$csprojPath -o bin --force --no-incremental -r win-x86 --packages .\.nuget  /p:RestoreFallbackFolders=$fallBackFolderPath --configfile NuGet.Config" -ForegroundColor yellow
    .\DotNet\cli\dotnet.exe build "$csprojPath" -o bin --force --no-incremental -r win-x86 --packages .\.nuget  /p:RestoreFallbackFolders="$fallBackFolderPath" --configfile NuGet.Config
}

function restorePackage($extension, $fallBackFolderPath) {
    if (Test-Path .\extensions.csproj -PathType Leaf) {
        Remove-Item .\extensions.csproj -Force
    }

    Copy-Item .\extensions.csproj_bak .\extensions.csproj -Force

    .\DotNet\cli\dotnet.exe add extensions.csproj package $extension.name -v $extension.version -n

    if (-Not (Test-Path ".\empty" -PathType Container)) {
        New-Item .\empty -ItemType Directory
    }

    dotnetBuild extensions.csproj $fallBackFolderPath
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
    $DotNetDir = Join-Path $rootPath -ChildPath "\DotNet\"
    $binFolderLocation = Join-Path $Currentlocation -ChildPath "\bin\"
    $emptyFolder = Join-Path $Currentlocation -ChildPath "\empty"
    $fallBackFolder = Join-Path $Currentlocation -ChildPath "\fallBackFolder"
    $extensionsJson = Join-Path $Currentlocation -ChildPath "\extensions.json"

    # Start with a clean slate, create dotnet folder
    if (-Not (Test-Path $DotNetDir)) {
        New-Item -ItemType Directory $DotNetDir
    }    

    # Check if cli is present
    $cliDir = Join-Path $DotNetDir -ChildPath "\cli\"
    if (-Not (Test-Path $cliDir)) {
        # Download dotnet CLI
        $dotnetCliDownloadUrl = "https://download.microsoft.com/download/1/2/E/12E2BC14-7A9F-4497-A351-02B7C2DDD599/dotnet-sdk-2.1.102-win-x86.zip"
        $dotnetCliZip = Join-Path $DotNetDir -ChildPath "dotnet.zip"
        Download $dotnetCliDownloadUrl $dotnetCliZip    

        # Unzip dotnet CLI    
        New-Item -ItemType Directory $cliDir
        Unzip $dotnetCliZip $cliDir
    }

    # get list of extensions to install
    $extensions = Get-Content -Raw -Path $extensionsJson  | ConvertFrom-Json

    Set-Item -Path Env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE -Value "true"

    # install all extensions one by one
    foreach ($extension in $extensions) {
        $message = "Generating Nuget Cache for extension:" + $extension.name
        LogAppVeyorMessage $message $runningInAppveyorEnv
        restorePackage $extension $emptyFolder
    }

    #clean up fallback and create fallback folder
    Get-ChildItem .\.nuget -Include *.nupkg -Recurse | Remove-Item
    Get-ChildItem .\.nuget -Include *.xml -Recurse | Foreach-Object {
        if (-Not $_.PSIsContainer) {
            Remove-Item $_
        }
    }

    if (Test-Path fallBackFolder) {
        Remove-Item .\fallBackFolder -Recurse
        sleep -Seconds 2
    }

    # renaming the nuget cache folder to Fallback
    Rename-Item .\.nuget fallBackFolder

    # for matching before and after
    foreach ($extension in $extensions) {
        if (Test-Path .\.nuget) {
            Remove-Item .\.nuget -Recurse
            sleep -Seconds 2
        }

        $message = "Verifying extension:" + $extension.name
        LogAppVeyorMessage $message $runningInAppveyorEnv

        # with fallback folder
        restorePackage $extension $fallBackFolder
        $binWith = Get-ChildItem -Recurse -path $binFolderLocation
        Remove-Item $binFolderLocation -Recurse

        if (Test-Path ".\.nuget") {
            Write-Host ".Nuget cache folder is not empty" -ForegroundColor red
            exit
        }

        # without fallback folder
        restorePackage $extension $emptyFolder
        $binWithOut = Get-ChildItem -Recurse -path $binFolderLocation
        Remove-Item $binFolderLocation -Recurse

        $binDiff = Compare-Object -ReferenceObject $binWith -DifferenceObject $binWithOut
        
        if (-Not ($binDiff -eq $null)) { 
            $message = "Restore with fallback folder does not match for " + $extension.name
            LogAppVeyorErrorMessage $message $runningInAppveyorEnv
        }
        else {
            $message = "Verification complete for" + $extension.name
            LogAppVeyorMessage $message $runningInAppveyorEnv
        }
    }

    

    if ($runningInAppveyorEnv) {
        $fallbackFolderVersion = $env:APPVEYOR_BUILD_VERSION
        $fallbackFolderParentDirectoryName = "FuncNugetFallback." + $fallbackFolderVersion
        $fallbackFolderParentDirectoryPath = Join-Path ((Get-Item $Currentlocation).Parent).FullName -ChildPath $fallbackFolderParentDirectoryName
    }
    else {
        $fallbackFolderVersion = "1.0.0"
        $fallbackFolderParentDirectoryName = "FuncNugetFallback." + $fallbackFolderVersion
        $fallbackFolderParentDirectoryPath = Join-Path $Currentlocation -ChildPath $fallbackFolderParentDirectoryName
    }

    $fallbackFolderChildDirectoryPath = Join-Path $Currentlocation -ChildPath $fallbackFolderVersion

    Rename-Item $fallBackFolder $fallbackFolderChildDirectoryPath
    New-Item $fallbackFolderParentDirectoryPath -ItemType Directory
    Move-Item $fallbackFolderChildDirectoryPath $fallbackFolderParentDirectoryPath

    $fallBackFolderZipPath = Join-Path $Currentlocation -ChildPath "$fallbackFolderParentDirectoryName.zip"

    if (Test-Path $fallBackFolderZipPath) {
        Remove-Item $fallBackFolderZipPath
    }
    
    Out-File -filepath ..\$fallbackFolderParentDirectoryName\$fallbackFolderVersion\marker.txt
    [IO.Compression.ZipFile]::CreateFromDirectory($fallbackFolderParentDirectoryPath, $fallBackFolderZipPath)
    
}
catch {
    LogErrorAndExit "UnKnown Error" $_.Exception
}