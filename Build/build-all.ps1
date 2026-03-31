<#
.SYNOPSIS
    PowerShell 7.x build script for Azure Functions Templates (npm-independent)
    
.DESCRIPTION
    This script replicates the gulp build-all task without npm dependencies.
    It performs the exact same steps in the exact same order:
    1. clean - Delete bin folder
    2. nuget-download - Download nuget.exe if not present
    3. nuget-pack - Pack all nuspec files
    4. unzip-templates - Extract nupkg files
    5. resources-convert - Convert .resx and .lcl files to JSON
    6. resources-build - Format resource JSON files
    7. resources-copy - Copy Resources.json as Resources.en-US.json
    8. userprompt-copy - Copy userPrompts.json files
    9. build-templates - Build templates.json (v1 format)
    10. build-templates-v2 - Build templates.json (v2 format)
    11. build-bindings - Build bindings.json with documentation
    12. zip-output - Create final zip files
    13. clean-temp - Clean temporary files

.PARAMETER BuildVersion
    The build version number (default: 1, or from devops_buildNumber env var)

.EXAMPLE
    .\build-all.ps1
    .\build-all.ps1 -BuildVersion "2.0.0"

.NOTES
    Requires PowerShell 7.0 or later
#>

#Requires -Version 7.0

param(
    [string]$BuildVersion = $null
)

$ErrorActionPreference = "Stop"
$PSNativeCommandUseErrorActionPreference = $true

$ScriptDir = $PSScriptRoot
$RootDir = Split-Path -Parent $ScriptDir
$BinDir = Join-Path $RootDir "bin"
$TempDir = Join-Path $BinDir "Temp"
$VSDir = Join-Path $BinDir "VS"
$ExtBundleDir = Join-Path $TempDir "ExtensionBundle"
$OutDir = Join-Path $TempDir "out"

# Determine build version - handle null or empty string
$BuildVersion = [string]::IsNullOrWhiteSpace($BuildVersion) ? ($env:devops_buildNumber ?? "1") : $BuildVersion

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Azure Functions Templates Build Script" -ForegroundColor Cyan
Write-Host "Build Version: $BuildVersion" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

#region Helper Functions

function Write-Step {
    param([string]$Message)
    $timestamp = Get-Date -Format "HH:mm:ss"
    Write-Host "[$timestamp] $Message" -ForegroundColor Green
}

function Write-StepComplete {
    param([string]$Message, [TimeSpan]$Duration)
    $timestamp = Get-Date -Format "HH:mm:ss"
    # PS7 ternary operator
    $durationStr = $Duration.TotalSeconds -lt 1 ? "$([int]$Duration.TotalMilliseconds) ms" : "$([math]::Round($Duration.TotalSeconds, 2)) s"
    Write-Host "[$timestamp] Finished '$Message' after $durationStr" -ForegroundColor Green
}

function Write-JsonFile {
    <#
    .SYNOPSIS
        Writes JSON to file with LF line endings (matching Node.js/gulp output)
    .NOTES
        Only normalizes actual file line endings, not escaped sequences within JSON strings.
        JSON escapes actual newlines as \n and \r\n as \r\n, so we only replace actual CRLF bytes.
    #>
    param(
        [Parameter(Mandatory)]
        [object]$InputObject,
        [Parameter(Mandatory)]
        [string]$Path,
        [int]$Depth = 10
    )
    
    # Convert to JSON
    $json = $InputObject | ConvertTo-Json -Depth $Depth
    
    # Only normalize actual file line endings (CRLF -> LF), not escaped sequences in JSON strings
    # PowerShell's ConvertTo-Json outputs with CRLF line endings on Windows
    # We need to convert those to LF to match Node.js output
    # Escaped \r\n in JSON strings appears as \\r\\n or \r\n literals - we don't want to touch those
    $json = $json -replace "`r`n", "`n"
    
    # Write with UTF8 encoding without BOM
    [System.IO.File]::WriteAllText($Path, $json, [System.Text.UTF8Encoding]::new($false))
}

function Convert-ResxToJson {
    <#
    .SYNOPSIS
        Converts a .resx XML file to JSON format
    .NOTES
        Uses XmlDocument with PreserveWhitespace to preserve CRLF line endings
        in string values, matching Node.js xml2js behavior.
    #>
    param([string]$ResxPath)
    
    # Use XmlDocument with PreserveWhitespace to preserve CRLF in values
    $doc = New-Object System.Xml.XmlDocument
    $doc.PreserveWhitespace = $true
    $doc.Load($ResxPath)
    
    $result = [ordered]@{}
    
    foreach ($data in $doc.SelectNodes("//data")) {
        $name = $data.GetAttribute("name")
        $valueNode = $data.SelectSingleNode("value")
        $value = if ($valueNode) { $valueNode.InnerText } else { $null }
        if ($name -and $null -ne $value) {
            $result[$name] = $value
        }
    }
    
    return $result
}

function Convert-LclToJson {
    <#
    .SYNOPSIS
        Converts an .lcl XML file to JSON format
    .NOTES
        Uses XmlDocument with PreserveWhitespace to preserve CRLF line endings
        in string values, matching Node.js xml2js behavior.
        LCL files use the LCX namespace which requires a namespace manager for XPath queries.
    #>
    param([string]$LclPath)
    
    $structuralItems = @('Resources.resx', 'Strings')
    $translationPrefix = ';'
    
    # Use XmlDocument with PreserveWhitespace to preserve CRLF in values
    $doc = New-Object System.Xml.XmlDocument
    $doc.PreserveWhitespace = $true
    $doc.Load($LclPath)
    
    # LCL files use the LCX namespace - need namespace manager for XPath
    $nsmgr = New-Object System.Xml.XmlNamespaceManager($doc.NameTable)
    $nsmgr.AddNamespace("lcx", "http://schemas.microsoft.com/locstudio/2006/6/lcx")
    
    $result = [ordered]@{}
    
    $items = $doc.SelectNodes("//lcx:Item", $nsmgr)
    foreach ($item in $items) {
        $itemId = $item.GetAttribute("ItemId")
        
        # Only process items starting with translation prefix
        if (-not $itemId -or -not $itemId.StartsWith($translationPrefix)) {
            continue
        }
        
        # Extract key name (remove leading prefix)
        $key = $itemId.Substring($translationPrefix.Length)
        
        # Skip structural items
        if ($structuralItems -contains $key) {
            continue
        }
        
        # Find Tgt element (with namespace)
        $tgt = $item.SelectSingleNode(".//lcx:Tgt", $nsmgr)
        if ($tgt) {
            $val = $tgt.SelectSingleNode(".//lcx:Val", $nsmgr)
            if ($val) {
                $result[$key] = $val.InnerText
            }
        }
    }
    
    return $result
}

function Get-FilesWithContent {
    <#
    .SYNOPSIS
        Gets file contents from a folder, excluding specified files.
        Returns an ordered dictionary preserving native NTFS order (matching Node.js fs.readdirSync).
    .NOTES
        Uses [System.IO.Directory]::GetFiles() instead of Get-ChildItem because:
        - Get-ChildItem sorts alphabetically by default
        - Node.js fs.readdirSync returns native NTFS B+ tree order
        - [System.IO.Directory]::GetFiles() returns the same native order as Node.js
    #>
    param(
        [string]$Folder,
        [string[]]$FilesToIgnore
    )
    
    $result = [ordered]@{}
    if (-not (Test-Path $Folder)) {
        return $result
    }
    
    # Use .NET Directory.GetFiles() to get native NTFS order (same as Node.js fs.readdirSync)
    # Do NOT use Get-ChildItem as it sorts alphabetically
    $filePaths = [System.IO.Directory]::GetFiles($Folder)
    foreach ($filePath in $filePaths) {
        $fileName = [System.IO.Path]::GetFileName($filePath)
        if ($FilesToIgnore -notcontains $fileName) {
            # Read raw bytes and decode to preserve BOM if present
            # [System.Text.Encoding]::UTF8 strips BOM, so we must detect and preserve it
            $bytes = [System.IO.File]::ReadAllBytes($filePath)
            if ($bytes.Length -eq 0) {
                $content = ""
            }
            elseif ($bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF) {
                # File has UTF-8 BOM - decode and prepend BOM character
                $content = [char]0xFEFF + [System.Text.Encoding]::UTF8.GetString($bytes, 3, $bytes.Length - 3)
            }
            else {
                $content = [System.Text.Encoding]::UTF8.GetString($bytes)
            }
            $result[$fileName] = $content
        }
    }
    
    return $result
}

#endregion

#region Build Tasks

function Invoke-Clean {
    Write-Step "Starting 'clean'..."
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    
    if (Test-Path $BinDir) {
        Remove-Item -Path $BinDir -Recurse -Force
    }
    
    $sw.Stop()
    Write-StepComplete "clean" $sw.Elapsed
}

function Invoke-NugetDownload {
    Write-Step "Starting 'nuget-download'..."
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    
    $nugetPath = Join-Path $ScriptDir "nuget.exe"
    if (-not (Test-Path $nugetPath)) {
        $nugetUrl = "https://dist.nuget.org/win-x86-commandline/v6.0.0/nuget.exe"
        Invoke-WebRequest -Uri $nugetUrl -OutFile $nugetPath
    }
    
    $sw.Stop()
    Write-StepComplete "nuget-download" $sw.Elapsed
}

function Invoke-NugetPack {
    Write-Step "Starting 'nuget-pack'..."
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    
    $nugetPath = Join-Path $ScriptDir "nuget.exe"
    $dotnetDir = Join-Path $ScriptDir "PackageFiles\Dotnet_precompiled"
    $extBundleSourceDir = Join-Path $ScriptDir "PackageFiles\ExtensionBundle"
    
    # Create output directories
    $null = New-Item -ItemType Directory -Path $VSDir -Force
    $null = New-Item -ItemType Directory -Path $ExtBundleDir -Force
    
    # Pack Dotnet_precompiled nuspec files (parallel)
    Get-ChildItem -Path $dotnetDir -Filter "*.nuspec" | ForEach-Object -Parallel {
        $nuget = $using:nugetPath
        $version = $using:BuildVersion
        $outDir = $using:VSDir
        Write-Host "Attempting to build package from '$($_.Name)'."
        & $nuget pack $_.FullName -Properties "patchVersion=$version" -OutputDirectory $outDir -NonInteractive | Out-Null
    } -ThrottleLimit 4
    
    # Pack ExtensionBundle nuspec files (parallel)
    Get-ChildItem -Path $extBundleSourceDir -Filter "*.nuspec" | ForEach-Object -Parallel {
        $nuget = $using:nugetPath
        $version = $using:BuildVersion
        $outDir = $using:ExtBundleDir
        Write-Host "Attempting to build package from '$($_.Name)'."
        & $nuget pack $_.FullName -Properties "patchVersion=$version" -OutputDirectory $outDir -NonInteractive | Out-Null
    } -ThrottleLimit 4
    
    $sw.Stop()
    Write-StepComplete "nuget-pack" $sw.Elapsed
}

function Invoke-UnzipTemplates {
    Write-Step "Starting 'unzip-templates'..."
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    
    # Extract nupkg files in parallel (they're just zip files)
    Get-ChildItem -Path $ExtBundleDir -Filter "*.nupkg" | ForEach-Object -Parallel {
        $destPath = Join-Path $using:TempDir "Temp-$($_.BaseName)"
        Expand-Archive -Path $_.FullName -DestinationPath $destPath -Force
    } -ThrottleLimit 4
    
    $sw.Stop()
    Write-StepComplete "unzip-templates" $sw.Elapsed
}

function Invoke-ResourcesConvert {
    Write-Step "Starting 'resources-convert'..."
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    
    $nupkgFiles = Get-ChildItem -Path $ExtBundleDir -Filter "*.nupkg"
    foreach ($nupkg in $nupkgFiles) {
        $fileName = $nupkg.BaseName
        $dirPath = Join-Path $TempDir "Temp-$fileName"
        $resourceFile = Join-Path $dirPath "Resources\Resources.resx"
        $convertPath = Join-Path $dirPath "resources-convert"
        
        if (-not (Test-Path $resourceFile)) {
            continue
        }
        
        New-Item -ItemType Directory -Path $convertPath -Force | Out-Null
        
        # Convert base Resources.resx to JSON
        $json = Convert-ResxToJson -ResxPath $resourceFile
        $jsonPath = Join-Path $convertPath "Resources.json"
        Write-JsonFile -InputObject $json -Path $jsonPath
        
        # Convert localized .resx files
        $resourcesDir = Join-Path $dirPath "Resources"
        $localizedDirs = Get-ChildItem -Path $resourcesDir -Directory -ErrorAction SilentlyContinue
        foreach ($localizedDir in $localizedDirs) {
            $localizedResx = Join-Path $localizedDir.FullName "Resources.resx"
            if (Test-Path $localizedResx) {
                $localizedJson = Convert-ResxToJson -ResxPath $localizedResx
                $localizedJsonPath = Join-Path $convertPath "Resources.$($localizedDir.Name).json"
                Write-JsonFile -InputObject $localizedJson -Path $localizedJsonPath
            }
        }
        
        # Convert .lcl files from Resources_lcl directory
        $resourcesLclPath = Join-Path $dirPath "Resources_lcl"
        if (Test-Path $resourcesLclPath) {
            $lclDirs = Get-ChildItem -Path $resourcesLclPath -Directory -ErrorAction SilentlyContinue
            foreach ($lclDir in $lclDirs) {
                $lclFile = Join-Path $lclDir.FullName "Resources.resx.lcl"
                if (Test-Path $lclFile) {
                    $lclJson = Convert-LclToJson -LclPath $lclFile
                    $lclJsonPath = Join-Path $convertPath "Resources.$($lclDir.Name).json"
                    Write-JsonFile -InputObject $lclJson -Path $lclJsonPath
                }
            }
        }
    }
    
    $sw.Stop()
    Write-StepComplete "resources-convert" $sw.Elapsed
}

function Invoke-ResourcesBuild {
    Write-Step "Starting 'resources-build'..."
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    
    $nupkgFiles = Get-ChildItem -Path $ExtBundleDir -Filter "*.nupkg"
    foreach ($nupkg in $nupkgFiles) {
        $fileName = $nupkg.BaseName
        $dirPath = Join-Path $TempDir "Temp-$fileName"
        $resourceFile = Join-Path $dirPath "Resources\Resources.resx"
        $convertPath = Join-Path $dirPath "resources-convert"
        
        if (-not (Test-Path $resourceFile)) {
            continue
        }
        
        $resourcesOutPath = Join-Path $OutDir "$fileName\resources"
        $resourcesV2OutPath = Join-Path $OutDir "$fileName\resources-v2"
        $null = New-Item -ItemType Directory -Path $resourcesOutPath -Force
        $null = New-Item -ItemType Directory -Path $resourcesV2OutPath -Force
        
        # Load English base resources
        $enJsonPath = Join-Path $convertPath "Resources.json"
        $enJson = Get-Content -Path $enJsonPath -Raw | ConvertFrom-Json
        
        # Process base English file (order: en)
        $baseOutput = [ordered]@{ en = $enJson }
        Write-JsonFile -InputObject $baseOutput -Path (Join-Path $resourcesOutPath "Resources.json")
        Write-JsonFile -InputObject $baseOutput -Path (Join-Path $resourcesV2OutPath "Resources.json")
        
        # Process localized resource files (order: lang, en)
        $localizedFiles = Get-ChildItem -Path $convertPath -Filter "Resources.*.json"
        foreach ($localizedFile in $localizedFiles) {
            $langJson = Get-Content -Path $localizedFile.FullName -Raw | ConvertFrom-Json
            $output = [ordered]@{
                lang = $langJson
                en = $enJson
            }
            Write-JsonFile -InputObject $output -Path (Join-Path $resourcesOutPath $localizedFile.Name)
            Write-JsonFile -InputObject $output -Path (Join-Path $resourcesV2OutPath $localizedFile.Name)
        }
    }
    
    $sw.Stop()
    Write-StepComplete "resources-build" $sw.Elapsed
}

function Invoke-ResourcesCopy {
    Write-Step "Starting 'resources-copy'..."
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    
    $nupkgFiles = Get-ChildItem -Path $ExtBundleDir -Filter "*.nupkg"
    foreach ($nupkg in $nupkgFiles) {
        $fileName = $nupkg.BaseName
        $dirPath = Join-Path $TempDir "Temp-$fileName"
        $resourceFile = Join-Path $dirPath "Resources\Resources.resx"
        
        if (-not (Test-Path $resourceFile)) {
            continue
        }
        
        $resourcesOutPath = Join-Path $OutDir "$fileName\resources"
        $resourcesV2OutPath = Join-Path $OutDir "$fileName\resources-v2"
        
        # Copy Resources.json as Resources.en-US.json
        $srcFile = Join-Path $resourcesOutPath "Resources.json"
        if (Test-Path $srcFile) {
            Copy-Item -Path $srcFile -Destination (Join-Path $resourcesOutPath "Resources.en-US.json") -Force
            Copy-Item -Path $srcFile -Destination (Join-Path $resourcesV2OutPath "Resources.en-US.json") -Force
        }
    }
    
    $sw.Stop()
    Write-StepComplete "resources-copy" $sw.Elapsed
}

function Invoke-UserpromptCopy {
    Write-Step "Starting 'userprompt-copy'..."
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    
    $nupkgFiles = Get-ChildItem -Path $ExtBundleDir -Filter "*.nupkg"
    foreach ($nupkg in $nupkgFiles) {
        $fileName = $nupkg.BaseName
        $dirPath = Join-Path $TempDir "Temp-$fileName"
        $userPromptFile = Join-Path $dirPath "Bindings-v2\userPrompts.json"
        
        if (-not (Test-Path $userPromptFile)) {
            continue
        }
        
        $bindingsV2OutPath = Join-Path $OutDir "$fileName\bindings-v2"
        New-Item -ItemType Directory -Path $bindingsV2OutPath -Force | Out-Null
        
        Copy-Item -Path $userPromptFile -Destination (Join-Path $bindingsV2OutPath "userPrompts.json") -Force
    }
    
    $sw.Stop()
    Write-StepComplete "userprompt-copy" $sw.Elapsed
}

function Invoke-BuildTemplates {
    Write-Step "Starting 'build-templates'..."
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    
    $nupkgFiles = Get-ChildItem -Path $ExtBundleDir -Filter "*.nupkg"
    foreach ($nupkg in $nupkgFiles) {
        $fileName = $nupkg.BaseName
        $dirPath = Join-Path $TempDir "Temp-$fileName"
        $templatesDir = Join-Path $dirPath "templates"
        
        if (-not (Test-Path $templatesDir)) {
            continue
        }
        
        $templateListJson = @()
        $templates = Get-ChildItem -Path $templatesDir -Directory
        
        foreach ($template in $templates) {
            $filePath = $template.FullName
            
            # Get files with content (excluding function.json and metadata.json)
            $files = Get-FilesWithContent -Folder $filePath -FilesToIgnore @('function.json', 'metadata.json')
            
            # Use ordered hashtable to match gulp output order: id, runtime, files, function, metadata
            $templateObj = [ordered]@{
                id = $template.Name
                runtime = "2"
                files = $files
            }
            
            # Load function.json and metadata.json
            $functionJsonPath = Join-Path $filePath "function.json"
            $metadataJsonPath = Join-Path $filePath "metadata.json"
            
            if (Test-Path $functionJsonPath) {
                $templateObj["function"] = Get-Content -Path $functionJsonPath -Raw | ConvertFrom-Json -AsHashtable
            }
            if (Test-Path $metadataJsonPath) {
                $templateObj["metadata"] = Get-Content -Path $metadataJsonPath -Raw | ConvertFrom-Json -AsHashtable
            }
            
            $templateListJson += $templateObj
        }
        
        $templatesOutPath = Join-Path $OutDir "$fileName\templates"
        New-Item -ItemType Directory -Path $templatesOutPath -Force | Out-Null
        
        $outputPath = Join-Path $templatesOutPath "templates.json"
        Write-JsonFile -InputObject $templateListJson -Path $outputPath
    }
    
    $sw.Stop()
    Write-StepComplete "build-templates" $sw.Elapsed
}

function Invoke-BuildTemplatesV2 {
    Write-Step "Starting 'build-templates-v2'..."
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    
    $nupkgFiles = Get-ChildItem -Path $ExtBundleDir -Filter "*.nupkg"
    foreach ($nupkg in $nupkgFiles) {
        $fileName = $nupkg.BaseName
        $dirPath = Join-Path $TempDir "Temp-$fileName"
        $templatesDir = Join-Path $dirPath "templates-v2"
        
        if (-not (Test-Path $templatesDir)) {
            continue
        }
        
        $templateListJson = @()
        $templates = Get-ChildItem -Path $templatesDir -Directory
        
        foreach ($template in $templates) {
            $filePath = $template.FullName
            $templateJsonPath = Join-Path $filePath "template.json"
            
            if (-not (Test-Path $templateJsonPath)) {
                continue
            }
            
            # Get files with content (excluding template.json)
            $files = Get-FilesWithContent -Folder $filePath -FilesToIgnore @('template.json')
            
            # Load template.json preserving order, then append id and files at end
            $templateContent = Get-Content -Path $templateJsonPath -Raw
            $templateObj = [System.Collections.Specialized.OrderedDictionary]::new()
            $parsed = $templateContent | ConvertFrom-Json
            
            # Copy properties in original order
            foreach ($prop in $parsed.PSObject.Properties) {
                $templateObj[$prop.Name] = $prop.Value
            }
            
            # Append id and files at the end (as gulp does)
            $templateObj["id"] = $template.Name
            $templateObj["files"] = $files
            
            $templateListJson += $templateObj
        }
        
        $templatesV2OutPath = Join-Path $OutDir "$fileName\templates-v2"
        New-Item -ItemType Directory -Path $templatesV2OutPath -Force | Out-Null
        
        $outputPath = Join-Path $templatesV2OutPath "templates.json"
        Write-JsonFile -InputObject $templateListJson -Path $outputPath
    }
    
    $sw.Stop()
    Write-StepComplete "build-templates-v2" $sw.Elapsed
}

function Invoke-BuildBindings {
    Write-Step "Starting 'build-bindings'..."
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    
    $documentationDir = Join-Path $RootDir "Functions.Templates\Documentation"
    
    $nupkgFiles = Get-ChildItem -Path $ExtBundleDir -Filter "*.nupkg"
    foreach ($nupkg in $nupkgFiles) {
        $fileName = $nupkg.BaseName
        $dirPath = Join-Path $TempDir "Temp-$fileName"
        $bindingsPath = Join-Path $dirPath "Bindings\bindings.json"
        
        if (-not (Test-Path $bindingsPath)) {
            continue
        }
        
        $bindingFile = Get-Content -Path $bindingsPath -Raw | ConvertFrom-Json -AsHashtable
        
        # Process documentation for each binding
        foreach ($binding in $bindingFile["bindings"]) {
            if ($binding["documentation"]) {
                $docPath = $binding["documentation"]
                $docFile = Split-Path -Leaf $docPath.Replace('\', '/')
                $fullDocPath = Join-Path $documentationDir $docFile
                
                if (Test-Path $fullDocPath) {
                    # Use [System.IO.File]::ReadAllText to properly handle empty files (returns "" not $null)
                    $docContent = [System.IO.File]::ReadAllText($fullDocPath, [System.Text.Encoding]::UTF8)
                    $binding["documentation"] = $docContent
                }
            }
        }
        
        $bindingsOutPath = Join-Path $OutDir "$fileName\bindings"
        New-Item -ItemType Directory -Path $bindingsOutPath -Force | Out-Null
        
        $outputPath = Join-Path $bindingsOutPath "bindings.json"
        Write-JsonFile -InputObject $bindingFile -Path $outputPath
    }
    
    $sw.Stop()
    Write-StepComplete "build-bindings" $sw.Elapsed
}

function Invoke-ZipOutput {
    Write-Step "Starting 'zip-output'..."
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    
    if (Test-Path $OutDir) {
        # Create zip files in parallel
        Get-ChildItem -Path $OutDir -Directory | ForEach-Object -Parallel {
            $zipPath = Join-Path $using:BinDir "$($_.Name).zip"
            $jsonFiles = Get-ChildItem -Path $_.FullName -Filter "*.json" -Recurse
            if ($jsonFiles.Count -gt 0) {
                Compress-Archive -Path "$($_.FullName)\*" -DestinationPath $zipPath -Force
            }
        } -ThrottleLimit 4
    }
    
    $sw.Stop()
    Write-StepComplete "zip-output" $sw.Elapsed
}

function Invoke-CleanTemp {
    Write-Step "Starting 'clean-temp'..."
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    
    if (Test-Path $TempDir) {
        Remove-Item -Path $TempDir -Recurse -Force
    }
    
    $sw.Stop()
    Write-StepComplete "clean-temp" $sw.Elapsed
}

#endregion

#region Main Execution

$totalSw = [System.Diagnostics.Stopwatch]::StartNew()

Write-Step "Starting 'build-all'..."

try {
    # Execute all tasks in order (same as gulp build-all)
    Invoke-Clean
    Invoke-NugetDownload
    Invoke-NugetPack
    Invoke-UnzipTemplates
    Invoke-ResourcesConvert
    Invoke-ResourcesBuild
    Invoke-ResourcesCopy
    Invoke-UserpromptCopy
    Invoke-BuildTemplates
    Invoke-BuildTemplatesV2
    Invoke-BuildBindings
    Invoke-ZipOutput
    Invoke-CleanTemp
    
    $totalSw.Stop()
    $totalMinutes = [math]::Round($totalSw.Elapsed.TotalMinutes, 2)
    Write-StepComplete "build-all" $totalSw.Elapsed
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "Build completed successfully!" -ForegroundColor Cyan
    Write-Host "Total time: $totalMinutes min" -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan
}
catch {
    Write-Host "Build failed: $_" -ForegroundColor Red
    exit 1
}

#endregion
