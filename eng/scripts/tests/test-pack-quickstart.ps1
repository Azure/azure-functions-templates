$ErrorActionPreference = 'Stop'

$scriptPath = Join-Path $PSScriptRoot '..\pack-quickstart.ps1'
. $scriptPath -TemplateId test-template -PackageVersion 1.0.0 -ExpectedCommit ('0' * 40)

function Assert-True {
    param([bool] $Condition, [string] $Message)
    if (-not $Condition) { throw $Message }
}

$repoRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..\..'))
$manifestPath = Join-Path $repoRoot 'Functions.Templates\Template-Manifest\manifest.json'
$schemaPath = Join-Path $repoRoot 'Functions.Templates\Template-Manifest\schemas\manifest.schema.json'
$manifestText = Get-Content -LiteralPath $manifestPath -Raw
Assert-True (Test-Json -Json $manifestText -SchemaFile $schemaPath) 'Existing manifest must remain valid.'

$candidate = $manifestText | ConvertFrom-Json -AsHashtable
$candidate.templates[0].packageReference = [ordered]@{
    schemaVersion = 1
    packageId = 'Microsoft.Azure.Functions.Quickstarts.test'
    version = '1.0.0'
    source = 'https://api.nuget.org/v3/index.json'
    templateIdentity = 'Microsoft.Azure.Functions.Quickstarts.test.Project'
}
Assert-True (Test-Json -Json ($candidate | ConvertTo-Json -Depth 100) -SchemaFile $schemaPath) `
    'A valid exact package nomination must satisfy the manifest schema.'
$candidate.templates[0].packageReference.version = '1.0.0-preview.1'
Assert-True (Test-Json -Json ($candidate | ConvertTo-Json -Depth 100) -SchemaFile $schemaPath) `
    'Normalized prerelease versions must satisfy the manifest schema.'
$candidate.templates[0].packageReference.version = '1.0.0+build'
Assert-True (-not (Test-Json -Json ($candidate | ConvertTo-Json -Depth 100) -SchemaFile $schemaPath)) `
    'Build metadata is not accepted in the normalized package version.'
$candidate.templates[0].packageReference.version = '1.0.0'
$candidate.templates[0].packageReference.source = 'https://user@example.test/feed'
Assert-True (-not (Test-Json -Json ($candidate | ConvertTo-Json -Depth 100) -SchemaFile $schemaPath)) `
    'Package feeds containing user information must be rejected.'
$candidate.templates[0].packageReference.source = 'https://api.nuget.org/v3/index.json'
$candidate.templates[0].packageReference.artifact = $null
Assert-True (-not (Test-Json -Json ($candidate | ConvertTo-Json -Depth 100) -SchemaFile $schemaPath)) `
    'Explicit artifact null must be rejected.'

$packageId = Get-QuickstartPackageId -Id 'http-trigger-python-azd'
Assert-True ($packageId -eq 'Microsoft.Azure.Functions.Quickstarts.http-trigger-python-azd') `
    'Package identity must be stable and derived from the manifest ID.'
$config = Get-QuickstartTemplateConfig -PackageId $packageId -TemplateId 'http-trigger-python-azd' `
    -DisplayName 'HTTP Trigger' -Language Python
Assert-True ($config.tags.type -eq 'project' -and $config.Keys.Count -eq 4) `
    'Generated config must be a parameterless project template.'
$csharpConfig = Get-QuickstartTemplateConfig -PackageId $packageId -TemplateId test `
    -DisplayName Test -Language CSharp
Assert-True ($csharpConfig.tags.language -eq 'C#') 'CSharp must map to the engine language tag.'
$branchRejected = $false
try {
    $null = Get-QuickstartPinnedCommit -Repository 'Azure/azure-functions-templates' `
        -Ref 'refs/heads/main' -Expected ('0' * 40)
} catch { $branchRejected = $true }
Assert-True $branchRejected 'Mutable branch refs must not be packaged as releases.'

$tempRoot = Join-Path ([IO.Path]::GetTempPath()) "pack-quickstart-tests-$([Guid]::NewGuid().ToString('N'))"
$null = New-Item -ItemType Directory -Path $tempRoot
try {
    Add-Type -AssemblyName System.IO.Compression
    $sourceZip = Join-Path $tempRoot 'source.zip'
    $stream = [IO.File]::Create($sourceZip)
    try {
        $archive = [System.IO.Compression.ZipArchive]::new($stream, [System.IO.Compression.ZipArchiveMode]::Create, $false)
        try {
            foreach ($pair in @(
                @{ Path = 'repo-abc/'; Text = '' },
                @{ Path = 'repo-abc/LICENSE'; Text = 'sample license' },
                @{ Path = 'repo-abc/office365App/app.py'; Text = 'print("sample")' },
                @{ Path = 'repo-abc/office365App/local.settings.json'; Text = '{"Values":{"AzureWebJobsStorage":"UseDevelopmentStorage=true","ApiKey":"%API_KEY%"}}' },
                @{ Path = 'repo-abc/sharepointApp/app.py'; Text = 'other sample' },
                @{ Path = 'repo-abc/secretApp/local.settings.json'; Text = '{"Values":{"AzureWebJobsStorage":"DefaultEndpointsProtocol=https;AccountName=example;AccountKey=not-a-real-key"}}' }
            )) {
                $entry = $archive.CreateEntry($pair.Path)
                if ($pair.Text) {
                    $writer = [IO.StreamWriter]::new($entry.Open())
                    try { $writer.Write($pair.Text) } finally { $writer.Dispose() }
                }
            }
        } finally { $archive.Dispose() }
    } finally { $stream.Dispose() }

    $archive = [System.IO.Compression.ZipFile]::OpenRead($sourceZip)
    try {
        $archiveRoot = Get-QuickstartArchiveRoot -Archive $archive
        Assert-True ($archiveRoot -eq 'repo-abc') 'Source archive root must resolve once.'
        $projectRoot = Join-Path $tempRoot 'out\content\quickstart'
        $legalRoot = Join-Path $tempRoot 'out\license'
        $null = New-Item -ItemType Directory -Path $projectRoot -Force
        Copy-QuickstartProjectContent -Archive $archive -ArchiveRoot $archiveRoot `
            -FolderPath 'office365App' -Destination $projectRoot -LegalDestination $legalRoot
        Assert-True (Test-Path (Join-Path $projectRoot 'app.py')) 'Selected subfolder content must be copied.'
        Assert-True (-not (Test-Path (Join-Path $projectRoot '..\..\sharepointApp'))) `
            'Sibling sample content must not be copied.'
        Assert-True (Test-Path (Join-Path $legalRoot 'LICENSE')) 'Root license must be retained for subfolder samples.'
        Assert-True ($script:QuickstartLicensePath -eq 'license/LICENSE') 'Nuspec license path must match packaged notice.'

        $secretRoot = Join-Path $tempRoot 'secret-out\content\quickstart'
        $null = New-Item -ItemType Directory -Path $secretRoot -Force
        $secretError = $null
        try {
            Copy-QuickstartProjectContent -Archive $archive -ArchiveRoot $archiveRoot `
                -FolderPath 'secretApp' -Destination $secretRoot -LegalDestination (Join-Path $tempRoot 'secret-out\license')
        } catch { $secretError = $_.Exception.Message }
        Assert-True ($secretError -like '*credential-like value*') `
            'Credential-like local settings must fail with the credential guard diagnostic.'
    } finally { $archive.Dispose() }

    $rootZip = Join-Path $tempRoot 'root-project.zip'
    $stream = [IO.File]::Create($rootZip)
    try {
        $archive = [System.IO.Compression.ZipArchive]::new($stream, [System.IO.Compression.ZipArchiveMode]::Create, $false)
        try {
            foreach ($pair in @(
                @{ Path = 'root-repo/'; Text = '' },
                @{ Path = 'root-repo/LICENSE'; Text = 'root license' },
                @{ Path = 'root-repo/function_app.py'; Text = 'app = True' },
                @{ Path = 'root-repo/host.json'; Text = '{}' }
            )) {
                $entry = $archive.CreateEntry($pair.Path)
                if ($pair.Text) {
                    $writer = [IO.StreamWriter]::new($entry.Open())
                    try { $writer.Write($pair.Text) } finally { $writer.Dispose() }
                }
            }
        } finally { $archive.Dispose() }
    } finally { $stream.Dispose() }

    $archive = [System.IO.Compression.ZipFile]::OpenRead($rootZip)
    try {
        $archiveRoot = Get-QuickstartArchiveRoot -Archive $archive
        foreach ($folderPath in @('.', '')) {
            $rootDestination = Join-Path $tempRoot "root-$([guid]::NewGuid().ToString('N'))\content\quickstart"
            $rootLegalDestination = Join-Path (Split-Path -Parent $rootDestination) 'license'
            $null = New-Item -ItemType Directory -Path $rootDestination -Force
            Copy-QuickstartProjectContent -Archive $archive -ArchiveRoot $archiveRoot `
                -FolderPath $folderPath -Destination $rootDestination -LegalDestination $rootLegalDestination
            Assert-True (Test-Path (Join-Path $rootDestination 'function_app.py')) `
                "Root folderPath '$folderPath' must include the root project."
            Assert-True (Test-Path (Join-Path $rootDestination 'LICENSE')) `
                "Root folderPath '$folderPath' must preserve its project license."
            Assert-True ($script:QuickstartLicensePath -eq 'content/quickstart/LICENSE') `
                "Root folderPath '$folderPath' must reference the preserved license."
        }
    } finally { $archive.Dispose() }

    $traversalZip = Join-Path $tempRoot 'traversal.zip'
    $stream = [IO.File]::Create($traversalZip)
    try {
        $archive = [System.IO.Compression.ZipArchive]::new($stream, [System.IO.Compression.ZipArchiveMode]::Create, $false)
        try { $null = $archive.CreateEntry('repo-abc/../outside.txt') } finally { $archive.Dispose() }
    } finally { $stream.Dispose() }
    $archive = [System.IO.Compression.ZipFile]::OpenRead($traversalZip)
    try {
        $threw = $false
        try { $null = Get-QuickstartArchiveRoot -Archive $archive } catch { $threw = $true }
        Assert-True $threw 'Archive traversal paths must be rejected.'
    } finally { $archive.Dispose() }

    foreach ($name in @('one', 'two')) {
        $stage = Join-Path $tempRoot $name
        $null = New-Item -ItemType Directory -Path (Join-Path $stage 'content\quickstart\.template.config') -Force
        $null = New-Item -ItemType Directory -Path (Join-Path $stage 'license') -Force
        [IO.File]::WriteAllText((Join-Path $stage 'content\quickstart\main.py'), 'print("same")')
        $testConfig = Get-QuickstartTemplateConfig -PackageId $packageId `
            -TemplateId 'http-trigger-python-azd' -DisplayName 'HTTP Trigger' -Language Python
        [IO.File]::WriteAllText((Join-Path $stage 'content\quickstart\.template.config\template.json'), `
            (($testConfig | ConvertTo-Json -Depth 5) + "`n"))
        [IO.File]::WriteAllText((Join-Path $stage 'license\LICENSE'), 'license')
        $packagePath = Join-Path $stage "$packageId.1.0.0.nupkg"
        Write-QuickstartNupkg -Stage $stage -PackageId $packageId -Version '1.0.0' `
            -LicensePath 'license/LICENSE' -PackagePath $packagePath
    }
    $first = Join-Path $tempRoot "one\$packageId.1.0.0.nupkg"
    $second = Join-Path $tempRoot "two\$packageId.1.0.0.nupkg"
    Assert-True ((Get-FileHash $first -Algorithm SHA256).Hash -eq (Get-FileHash $second -Algorithm SHA256).Hash) `
        'Identical package inputs must produce identical archive bytes.'

    $legacyGermanOrder = @('content/quickstart/z.txt', "content/quickstart/$([char]0x00E4).txt") |
        Sort-Object -Culture de-DE
    $legacySwedishOrder = @('content/quickstart/z.txt', "content/quickstart/$([char]0x00E4).txt") |
        Sort-Object -Culture sv-SE
    Assert-True (($legacyGermanOrder -join "`n") -ne ($legacySwedishOrder -join "`n")) `
        'Cross-culture fixture paths must distinguish German and Swedish collation.'

    $previousCulture = [System.Threading.Thread]::CurrentThread.CurrentCulture
    $previousUICulture = [System.Threading.Thread]::CurrentThread.CurrentUICulture
    try {
        $cultureOutputs = @{}
        foreach ($cultureName in @('de-DE', 'sv-SE')) {
            [System.Threading.Thread]::CurrentThread.CurrentCulture = [Globalization.CultureInfo]::GetCultureInfo($cultureName)
            [System.Threading.Thread]::CurrentThread.CurrentUICulture = [Globalization.CultureInfo]::GetCultureInfo($cultureName)
            $stage = Join-Path $tempRoot "culture-$cultureName"
            $null = New-Item -ItemType Directory -Path (Join-Path $stage 'content\quickstart') -Force
            $null = New-Item -ItemType Directory -Path (Join-Path $stage 'license') -Force
            [IO.File]::WriteAllText((Join-Path $stage 'content\quickstart\z.txt'), 'z')
            [IO.File]::WriteAllText((Join-Path $stage "content\quickstart\$([char]0x00E4).txt"), 'accented')
            [IO.File]::WriteAllText((Join-Path $stage 'license\LICENSE'), 'license')
            $packagePath = Join-Path $stage "$packageId.1.0.0.nupkg"
            Write-QuickstartNupkg -Stage $stage -PackageId $packageId -Version '1.0.0' `
                -LicensePath 'license/LICENSE' -PackagePath $packagePath
            $inventory = Get-QuickstartFilesManifest -ProjectRoot $stage -ExcludePath $packagePath
            $archive = [System.IO.Compression.ZipFile]::OpenRead($packagePath)
            try { $archivePaths = @($archive.Entries | ForEach-Object FullName) } finally { $archive.Dispose() }
            $cultureOutputs[$cultureName] = @{
                packageHash = (Get-FileHash $packagePath -Algorithm SHA256).Hash
                inventory = @($inventory | ConvertTo-Json -Depth 10 -Compress)
                inventoryPaths = @($inventory | ForEach-Object path)
                archivePaths = $archivePaths
            }
        }
    } finally {
        [System.Threading.Thread]::CurrentThread.CurrentCulture = $previousCulture
        [System.Threading.Thread]::CurrentThread.CurrentUICulture = $previousUICulture
    }
    Assert-True ($cultureOutputs['de-DE'].packageHash -eq $cultureOutputs['sv-SE'].packageHash) `
        'Identical files must produce identical package bytes across cultures.'
    Assert-True (($cultureOutputs['de-DE'].inventory -join "`n") -eq ($cultureOutputs['sv-SE'].inventory -join "`n")) `
        'Identical files must produce identical inventory bytes across cultures.'
    Assert-True (($cultureOutputs['de-DE'].inventoryPaths -join "`n") -eq ($cultureOutputs['sv-SE'].inventoryPaths -join "`n")) `
        'Inventory paths must retain identical ordinal order across cultures.'
    Assert-True (($cultureOutputs['de-DE'].archivePaths -join "`n") -eq ($cultureOutputs['sv-SE'].archivePaths -join "`n")) `
        'Archive entries must retain identical ordinal order across cultures.'

    $package = [System.IO.Compression.ZipFile]::OpenRead($first)
    try {
        Assert-True ($package.GetEntry("$packageId.nuspec") -ne $null) 'Package must contain its nuspec.'
        Assert-True ($package.GetEntry('content/quickstart/main.py') -ne $null) `
            'Package must place project files under the template content root.'
        Assert-True ($package.GetEntry('content/quickstart/.template.config/template.json') -ne $null) `
            'Package must include the engine template config.'
    } finally { $package.Dispose() }

    $previousCliHome = $env:DOTNET_CLI_HOME
    $previousTelemetryOptOut = $env:DOTNET_CLI_TELEMETRY_OPTOUT
    $previousNoLogo = $env:DOTNET_NOLOGO
    $env:DOTNET_CLI_HOME = Join-Path $tempRoot 'dotnet-home'
    $env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
    $env:DOTNET_NOLOGO = '1'
    $installOutput = & dotnet new install $first 2>&1
    if ($LASTEXITCODE -ne 0 -or ($installOutput -join ' ') -match 'validation errors|No templates were found') {
        throw "dotnet new install rejected the generated package: $($installOutput -join ' ')"
    }
    $listOutput = & dotnet new list http-trigger-python-azd 2>&1
    if ($LASTEXITCODE -ne 0 -or ($listOutput -join ' ') -notmatch 'http-trigger-python-azd') {
        throw "dotnet new did not enumerate the installed project template: $($listOutput -join ' ')"
    }
    $generatedPath = Join-Path $tempRoot 'generated'
    $createOutput = & dotnet new http-trigger-python-azd --output $generatedPath 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet new could not instantiate the project template: $($createOutput -join ' ')"
    }
    Assert-True (Test-Path (Join-Path $generatedPath 'main.py')) 'Template engine must materialize project files.'
    $env:DOTNET_CLI_HOME = $previousCliHome
    $env:DOTNET_CLI_TELEMETRY_OPTOUT = $previousTelemetryOptOut
    $env:DOTNET_NOLOGO = $previousNoLogo
}
finally {
    Remove-Item -LiteralPath $tempRoot -Recurse -Force
}

Write-Host 'Quickstart packager tests passed.'
