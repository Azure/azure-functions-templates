param([string] $PackageEvidenceRoot)

$ErrorActionPreference = 'Stop'

$scriptPath = Join-Path $PSScriptRoot '..\pack-quickstart.ps1'
. $scriptPath -TemplateId test-template -PackageVersion 1.0.0 -ExpectedCommit ('0' * 40)

function Assert-True {
    param([bool] $Condition, [string] $Message)
    if (-not $Condition) { throw $Message }
}

function Assert-Rejected {
    param([scriptblock] $Action, [string] $MessagePattern)
    $failure = $null
    try { & $Action } catch { $failure = $_.Exception.Message }
    Assert-True ($failure -like $MessagePattern) "Expected rejection '$MessagePattern', got '$failure'."
}

function Assert-FuncConfig {
    param($Config, [string] $ExpectedPath)
    Assert-True (($Config.Keys | Sort-Object) -join ',' -ceq 'constraints,identity,name,postActions,primaryOutputs,shortName,tags') `
        'Config must have exactly the accepted metadata keys, with no user symbols or transformations.'
    Assert-True ($Config.tags.type -ceq 'project' -and $Config.tags.language -ceq 'Python') 'Project tags must be canonical.'
    Assert-True ($Config.constraints.Keys.Count -eq 1 -and $Config.constraints.host.Keys.Count -eq 2 -and
        $Config.constraints.host.type -ceq 'host' -and $Config.constraints.host.args.Count -eq 1 -and
        $Config.constraints.host.args[0].Count -eq 1 -and $Config.constraints.host.args[0].hostname -ceq 'func') `
        'Only the func host constraint is allowed, without invented workload constraints.'
    Assert-True ($Config.primaryOutputs.Count -eq 1 -and $Config.primaryOutputs[0].Count -eq 1 -and
        $Config.primaryOutputs[0].path -ceq $ExpectedPath) 'Primary output must be literal, unconditional, and point at the actual host root.'
    Assert-True ($Config.postActions.Count -eq 1) 'Exactly one required config action is allowed.'
    $action = $Config.postActions[0]
    Assert-True ($action.Count -eq 3 -and $action.actionId -ceq '00e33184-ffc7-43ef-bec0-b1684df8ad56' -and
        $action.continueOnError -is [bool] -and -not $action.continueOnError) 'Config action must fail closed.'
    Assert-True ($action.args.Count -eq 3 -and $action.args.primaryOutputIndex -is [string] -and
        $action.args.primaryOutputIndex -ceq '0' -and $action.args.stack -ceq 'python' -and
        $action.args.language -ceq 'Python') 'Action arguments must be exactly the canonical index, stack, and language.'
}

function Assert-FuncPackage {
    param([string] $Path, [string] $ExpectedHostPath)
    $package = [System.IO.Compression.ZipFile]::OpenRead($Path)
    try {
        $nuspecs = @($package.Entries | Where-Object { $_.FullName -match '^[^/]+\.nuspec$' })
        Assert-True ($nuspecs.Count -eq 1) 'Package must contain exactly one root nuspec.'
        $reader = [IO.StreamReader]::new($nuspecs[0].Open())
        try { [xml] $nuspec = $reader.ReadToEnd() } finally { $reader.Dispose() }
        $types = @($nuspec.package.metadata.packageTypes.packageType | ForEach-Object name)
        Assert-True (($types -join ',') -ceq 'Template,FuncTemplate') 'Package must declare exactly Template and FuncTemplate, not Workload or FuncRid.'
        $configEntry = $package.GetEntry('content/quickstart/.template.config/template.json')
        Assert-True ($null -ne $configEntry) 'Package must include the engine template config.'
        $reader = [IO.StreamReader]::new($configEntry.Open())
        try { $packedConfig = $reader.ReadToEnd() | ConvertFrom-Json -AsHashtable } finally { $reader.Dispose() }
        Assert-FuncConfig $packedConfig $ExpectedHostPath
        $hosts = @($package.Entries | Where-Object { $_.FullName -match '^content/quickstart/(.*/)?host\.json$' })
        Assert-True ($hosts.Count -eq 1 -and $hosts[0].FullName -ceq "content/quickstart/$ExpectedHostPath") `
            'The literal primary output must identify the sole actual copied host file.'
        Assert-True (@($package.Entries | Where-Object { $_.FullName -match '(^|/)\.func/config\.json$' }).Count -eq 0) `
            'Producer must not fabricate Func configuration at any project root.'
    } finally { $package.Dispose() }
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
    $metadataRoot = Join-Path $tempRoot 'metadata'
    $null = New-Item -ItemType Directory -Path $metadataRoot
    $configArgs = @{
        PackageId = $packageId
        TemplateId = 'http-trigger-python-azd'
        DisplayName = 'HTTP Trigger'
        Language = 'Python'
        Stack = 'python'
        ProjectRoot = $metadataRoot
    }
    Assert-Rejected { Get-QuickstartTemplateConfig @configArgs } '*exactly one host.json*found 0*'
    [IO.File]::WriteAllText((Join-Path $metadataRoot 'host.json'), '{}')
    $config = Get-QuickstartTemplateConfig @configArgs
    Assert-FuncConfig $config 'host.json'
    $null = New-Item -ItemType Directory -Path (Join-Path $metadataRoot 'src')
    [IO.File]::WriteAllText((Join-Path $metadataRoot 'src\host.json'), '{}')
    Assert-Rejected { Get-QuickstartTemplateConfig @configArgs } '*exactly one host.json*found 2*'
    Remove-Item -LiteralPath (Join-Path $metadataRoot 'host.json')
    Assert-FuncConfig (Get-QuickstartTemplateConfig @configArgs) 'src/host.json'
    foreach ($invalidHost in @('[]', '[{}]', 'null', '"host"', '42', '')) {
        [IO.File]::WriteAllText((Join-Path $metadataRoot 'src\host.json'), $invalidHost)
        Assert-Rejected { Get-QuickstartTemplateConfig @configArgs } '*must contain a JSON object*'
    }
    [IO.File]::WriteAllText((Join-Path $metadataRoot 'src\host.json'), '{invalid')
    Assert-Rejected { Get-QuickstartTemplateConfig @configArgs } '*Conversion from JSON failed*'
    [IO.File]::WriteAllText((Join-Path $metadataRoot 'src\host.json'), '{}')
    [IO.File]::Move((Join-Path $metadataRoot 'src\host.json'), (Join-Path $metadataRoot 'src\Host.json'))
    Assert-Rejected { Get-QuickstartTemplateConfig @configArgs } '*literal name host.json*'
    [IO.File]::Move((Join-Path $metadataRoot 'src\Host.json'), (Join-Path $metadataRoot 'src\host.json'))
    $null = New-Item -ItemType Directory -Path (Join-Path $metadataRoot 'unsupported root')
    [IO.File]::Move((Join-Path $metadataRoot 'src\host.json'), (Join-Path $metadataRoot 'unsupported root\host.json'))
    Assert-Rejected { Get-QuickstartTemplateConfig @configArgs } '*Unsupported nonliteral host.json output path*'
    [IO.File]::Move((Join-Path $metadataRoot 'unsupported root\host.json'), (Join-Path $metadataRoot 'src\host.json'))
    foreach ($pair in @(@('Python', 'Python'), @('python', 'python'), @('node', 'Python'))) {
        $configArgs.Stack = $pair[0]
        $configArgs.Language = $pair[1]
        Assert-Rejected { Get-QuickstartTemplateConfig @configArgs } '*Only the reviewed installed-catalog pair*'
    }
    $configArgs.Stack = 'python'
    $configArgs.Language = 'Python'
    Assert-Rejected {
        Invoke-QuickstartPack -Id http-trigger-python-azd -Version 1.0.0 -Expected ('0' * 40) `
            -Feed https://api.nuget.org/v3/index.json -OutPath $tempRoot
    } '*Explicit reviewed installed-catalog values*'
    foreach ($pair in @(@('', 'Python'), @('python', ''), @('node', 'Python'), @('python', 'python'))) {
        Assert-Rejected {
            Invoke-QuickstartPack -Id http-trigger-python-azd -Version 1.0.0 -Expected ('0' * 40) `
                -Feed https://api.nuget.org/v3/index.json -OutPath $tempRoot -Stack $pair[0] -Language $pair[1]
        } '*Explicit reviewed installed-catalog values*'
    }
    Assert-Rejected {
        Invoke-QuickstartPack -Id http-trigger-javascript-azd -Version 1.0.0 -Expected ('0' * 40) `
            -Feed https://api.nuget.org/v3/index.json -OutPath $tempRoot -Stack python -Language Python
    } '*Explicit reviewed installed-catalog values*'

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
        [IO.File]::WriteAllText((Join-Path $stage 'content\quickstart\host.json'), '{}')
        $testConfig = Get-QuickstartTemplateConfig -PackageId $packageId `
            -TemplateId 'http-trigger-python-azd' -DisplayName 'HTTP Trigger' -Language Python `
            -Stack python -ProjectRoot (Join-Path $stage 'content\quickstart')
        [IO.File]::WriteAllText((Join-Path $stage 'content\quickstart\.template.config\template.json'), `
            (($testConfig | ConvertTo-Json -Depth 10) + "`n"))
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
        Assert-True ($package.GetEntry('content/quickstart/main.py') -ne $null) `
            'Package must place project files under the template content root.'
    } finally { $package.Dispose() }
    Assert-FuncPackage $first 'host.json'

    if ($PackageEvidenceRoot) {
        foreach ($sample in @(
            @{ Id = 'http-trigger-python-azd'; Version = '1.0.0'; HostPath = 'host.json' },
            @{ Id = 'http-trigger-python-azd'; Version = '1.0.1'; HostPath = 'host.json' },
            @{ Id = 'timer-trigger-python-azd'; Version = '1.0.0'; HostPath = 'src/host.json' }
        )) {
            $sampleDirectory = Join-Path $PackageEvidenceRoot "$($sample.Id)\$($sample.Version)"
            $samplePackage = Join-Path $sampleDirectory "Microsoft.Azure.Functions.Quickstarts.$($sample.Id).$($sample.Version).nupkg"
            Assert-FuncPackage $samplePackage $sample.HostPath
            $release = Get-Content -LiteralPath (Join-Path $sampleDirectory 'release.json') -Raw | ConvertFrom-Json
            Assert-True ($release.packageSha256 -ceq (Get-FileHash -LiteralPath $samplePackage -Algorithm SHA256).Hash.ToLowerInvariant()) `
                'Retained real package must match its new-epoch release digest.'
        }
    }

    $previousCliHome = $env:DOTNET_CLI_HOME
    $previousTelemetryOptOut = $env:DOTNET_CLI_TELEMETRY_OPTOUT
    $previousNoLogo = $env:DOTNET_NOLOGO
    $env:DOTNET_CLI_HOME = Join-Path $tempRoot 'dotnet-home'
    $env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
    $env:DOTNET_NOLOGO = '1'
    $installOutput = & dotnet new install $first 2>&1
    if ($LASTEXITCODE -ne 0 -or ($installOutput -join ' ') -match 'validation errors') {
        throw "dotnet new install rejected the generated package: $($installOutput -join ' ')"
    }
    $generatedPath = Join-Path $tempRoot 'generated'
    $createOutput = & dotnet new http-trigger-python-azd --output $generatedPath 2>&1
    Assert-True ($LASTEXITCODE -ne 0 -and ($createOutput -join ' ') -match 'constraint') `
        "dotnet host must reject the func-only constraint: $($createOutput -join ' ')"
    Assert-True (-not (Test-Path (Join-Path $generatedPath 'main.py'))) 'dotnet host must not materialize func-only content.'
    $env:DOTNET_CLI_HOME = $previousCliHome
    $env:DOTNET_CLI_TELEMETRY_OPTOUT = $previousTelemetryOptOut
    $env:DOTNET_NOLOGO = $previousNoLogo
}
finally {
    Remove-Item -LiteralPath $tempRoot -Recurse -Force
}

Write-Host 'Quickstart packager tests passed.'
