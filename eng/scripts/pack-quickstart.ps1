<#
.SYNOPSIS
    Creates a local NuGet project-template package from one pinned manifest entry.

.DESCRIPTION
    This manual packager does not run sample code, restore dependencies, publish
    packages, or update the production manifest. It emits an unqualified release
    record and a candidate manifest alongside the package.

.EXAMPLE
    .\eng\scripts\pack-quickstart.ps1 -TemplateId http-trigger-python-azd `
      -PackageVersion 1.0.0 -ExpectedCommit <40-character-commit>
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string] $TemplateId,

    [Parameter(Mandatory)]
    [string] $PackageVersion,

    [Parameter(Mandatory)]
    [ValidatePattern('^[0-9a-fA-F]{40}$')]
    [string] $ExpectedCommit,

    [string] $SourceRef,

    [ValidatePattern('^https://')]
    [string] $PackageSource = 'https://api.nuget.org/v3/index.json',

    [string] $OutputPath = (Join-Path $PSScriptRoot '..\..\artifacts\quickstarts')
)

$ErrorActionPreference = 'Stop'
$script:QuickstartPackagerDotSourced = $MyInvocation.InvocationName -eq '.'

function Get-QuickstartPackageId {
    param([Parameter(Mandatory)][string] $Id)

    $packageId = "Microsoft.Azure.Functions.Quickstarts.$Id"
    if ($packageId.Length -gt 100 -or $packageId -notmatch '^[A-Za-z0-9][A-Za-z0-9_.-]*$') {
        throw "Template id '$Id' cannot be mapped to a NuGet package id. Use a package-safe manifest ID before packaging."
    }
    return $packageId
}

function Get-QuickstartTemplateConfig {
    param(
        [Parameter(Mandatory)][string] $PackageId,
        [Parameter(Mandatory)][string] $TemplateId,
        [Parameter(Mandatory)][string] $DisplayName,
        [Parameter(Mandatory)][string] $Language
    )

    $engineLanguage = if ($Language -eq 'CSharp') { 'C#' } else { $Language }
    return [ordered]@{
        identity  = "$PackageId.Project"
        name      = $DisplayName
        shortName = $TemplateId
        tags      = [ordered]@{
            language = $engineLanguage
            type     = 'project'
        }
    }
}

function Get-QuickstartManifestEntry {
    param([Parameter(Mandatory)][string] $ManifestPath, [Parameter(Mandatory)][string] $Id)

    $manifest = Get-Content -LiteralPath $ManifestPath -Raw | ConvertFrom-Json -AsHashtable
    $matches = @($manifest.templates | Where-Object { $_.id -ceq $Id })
    if ($matches.Count -ne 1) {
        throw "Expected exactly one manifest entry with id '$Id', found $($matches.Count)."
    }
    $duplicateIds = @($manifest.templates | Group-Object { $_.id.ToLowerInvariant() } | Where-Object Count -gt 1)
    if ($duplicateIds.Count -gt 0) {
        throw 'Manifest contains case-insensitive duplicate template IDs.'
    }
    return $matches[0]
}

function Get-QuickstartArchiveRoot {
    param([Parameter(Mandatory)][System.IO.Compression.ZipArchive] $Archive)

    $paths = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
    $roots = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    $files = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
    $totalBytes = [long]0
    if ($Archive.Entries.Count -gt 10000) {
        throw 'Source archive contains more than 10,000 entries.'
    }
    foreach ($entry in $Archive.Entries) {
        $name = $entry.FullName
        $normalizedName = $name.TrimEnd('/')
        $segments = $normalizedName.Split('/')
        if ($name.Contains('\') -or $name.StartsWith('/') -or $name -match '(^|/)\.\.?(/|$)' -or
            $name -match '^[A-Za-z]:' -or $name -match '(^|/)[^/]*:' -or
            $segments.Count -eq 0 -or $segments -contains '') {
            throw "Source archive contains unsafe path '$name'."
        }
        if (-not $paths.Add($normalizedName)) {
            throw "Source archive contains a case-insensitive path collision at '$name'."
        }
        for ($index = 1; $index -lt $segments.Count; $index++) {
            $parent = ($segments[0..($index - 1)] -join '/')
            if ($files.Contains($parent)) {
                throw "Source archive path '$name' is nested below file '$parent'."
            }
        }
        if (-not $name.EndsWith('/')) {
            $files.Add($normalizedName) | Out-Null
            if (@($paths | Where-Object { $_.StartsWith("$normalizedName/", [StringComparison]::OrdinalIgnoreCase) }).Count -gt 0) {
                throw "Source archive file '$name' is also used as a directory."
            }
        }
        $mode = ($entry.ExternalAttributes -shr 16) -band 0xF000
        if ($mode -eq 0xA000) {
            throw "Source archive contains symbolic link '$name'."
        }
        $totalBytes += $entry.Length
        if ($entry.Length -gt 50MB -or $totalBytes -gt 512MB) {
            throw 'Source archive exceeds the 50 MiB per-file or 512 MiB total uncompressed limit.'
        }
        if ($name -match '^[^/]+/') {
            $null = $roots.Add($name.Split('/')[0])
        } else {
            throw "Source archive path '$name' is outside the expected single root directory."
        }
    }
    if ($roots.Count -ne 1) {
        throw "Expected one source archive root, found $($roots.Count)."
    }
    return @($roots)[0]
}

function Copy-QuickstartProjectContent {
    param(
        [Parameter(Mandatory)][System.IO.Compression.ZipArchive] $Archive,
        [Parameter(Mandatory)][string] $ArchiveRoot,
        [Parameter(Mandatory)][AllowEmptyString()][string] $FolderPath,
        [Parameter(Mandatory)][string] $Destination,
        [Parameter(Mandatory)][string] $LegalDestination
    )

    $folder = $FolderPath.Trim('/').Trim('\')
    if ($folder -in @('.', '')) { $folder = '' }
    if ($folder -match '(^|[\\/])\.\.?([\\/]|$)' -or $folder.Contains(':')) {
        throw "Manifest folderPath '$FolderPath' is not a safe relative path."
    }
    $prefix = if ($folder) { "$ArchiveRoot/$folder/" } else { "$ArchiveRoot/" }
    $selected = @($Archive.Entries | Where-Object {
        $_.FullName.StartsWith($prefix, [StringComparison]::Ordinal) -and
        -not $_.FullName.EndsWith('/')
    })
    if ($selected.Count -eq 0) {
        throw "Manifest folderPath '$FolderPath' has no files at the pinned source commit."
    }

    $copied = 0
    $selectedLicensePath = $null
    $totalBytes = [long]0
    foreach ($entry in $selected) {
        $relative = $entry.FullName.Substring($prefix.Length)
        if (-not $relative -or $relative -match '(^|/)\.git(/|$)') { continue }
        if ($relative -match '(^|/)\.template\.config(/|$)') {
            throw "Source folder already has template engine metadata '$relative'."
        }
        $totalBytes += $entry.Length
        if ($totalBytes -gt 256MB) {
            throw 'Selected project content exceeds the 256 MiB uncompressed limit.'
        }
        $destinationPath = [IO.Path]::GetFullPath((Join-Path $Destination ($relative.Replace('/', [IO.Path]::DirectorySeparatorChar))))
        $destinationRoot = [IO.Path]::GetFullPath($Destination).TrimEnd('\', '/') + [IO.Path]::DirectorySeparatorChar
        if (-not $destinationPath.StartsWith($destinationRoot, [StringComparison]::OrdinalIgnoreCase)) {
            throw "Archive path '$relative' escapes the selected project folder."
        }
        $null = New-Item -ItemType Directory -Path (Split-Path -Parent $destinationPath) -Force
        $input = $entry.Open()
        try {
            $output = [IO.File]::Create($destinationPath)
            try { $input.CopyTo($output) } finally { $output.Dispose() }
        } finally { $input.Dispose() }
        if ([IO.Path]::GetFileName($relative) -match '^(?i:LICENSE)(\..*)?$') {
            $selectedLicensePath = "content/quickstart/$relative"
        }
        $copied++
    }
    if ($copied -eq 0) {
        throw "Manifest folderPath '$FolderPath' contains no project files after excluding repository metadata."
    }

    if ($selectedLicensePath) {
        $script:QuickstartLicensePath = $selectedLicensePath
    } else {
        $legalEntries = @($Archive.Entries | Where-Object {
            $_.FullName -match ("^" + [regex]::Escape($ArchiveRoot) + "/(?i:LICENSE|NOTICE)(\.[^/]*)?$") -and
            -not $_.FullName.EndsWith('/')
        })
        $rootLicenses = @($legalEntries | Where-Object {
            [IO.Path]::GetFileName($_.FullName) -match '^(?i:LICENSE)(\..*)?$'
        })
        if ($rootLicenses.Count -eq 0) {
            throw "No LICENSE file exists in the selected project or repository root for '$FolderPath'."
        }
        $null = New-Item -ItemType Directory -Path $LegalDestination -Force
        foreach ($entry in $legalEntries) {
            $target = Join-Path $LegalDestination ([IO.Path]::GetFileName($entry.FullName))
            $input = $entry.Open()
            try {
                $output = [IO.File]::Create($target)
                try { $input.CopyTo($output) } finally { $output.Dispose() }
            } finally { $input.Dispose() }
        }
        $script:QuickstartLicensePath = "license/$([IO.Path]::GetFileName($rootLicenses[0].FullName))"
    }

    $settingsPath = Join-Path $Destination 'local.settings.json'
    if (Test-Path -LiteralPath $settingsPath) {
        $settings = Get-Content -LiteralPath $settingsPath -Raw | ConvertFrom-Json
        foreach ($property in $settings.Values.PSObject.Properties) {
            $value = [string]$property.Value
            if ($value -match '(?i)(AccountKey=|SharedAccessKey=|Password=|client_secret|api[_-]?key\s*[:=])' -and
                $value -notmatch '(?i)(<[^>]+>|\$\{[^}]+\}|%[^%]+%)') {
                throw "local.settings.json contains a credential-like value in '$($property.Name)'."
            }
        }
    }
}

function Invoke-GitHubApi {
    param([Parameter(Mandatory)][string[]] $Arguments)

    $response = & gh api @Arguments 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw "GitHub API request failed: $($response -join ' ')"
    }
    return ($response -join "`n") | ConvertFrom-Json
}

function Get-QuickstartPinnedCommit {
    param(
        [Parameter(Mandatory)][string] $Repository,
        [Parameter(Mandatory)][string] $Ref,
        [Parameter(Mandatory)][string] $Expected
    )

    if ($Ref -notmatch '^refs/(tags|heads)/(.+)$') {
        throw "Source ref '$Ref' must be fully qualified."
    }
    if ($Matches[1] -ne 'tags') {
        throw 'Packaging requires a signed annotated tag. Branch-backed manifest entries must be reviewed and retagged before release.'
    }
    $tag = [Uri]::EscapeDataString($Matches[2])
    $reference = Invoke-GitHubApi -Arguments @("repos/$Repository/git/ref/tags/$tag")
    if ($reference.object.type -ne 'tag') {
        throw "Source ref '$Ref' is not an annotated tag."
    }
    $tagObject = Invoke-GitHubApi -Arguments @("repos/$Repository/git/tags/$($reference.object.sha)")
    if ($tagObject.verification.verified -ne $true -or $tagObject.object.type -ne 'commit') {
        throw "Source tag '$Ref' is not GitHub-verified or does not point directly to a commit."
    }
    if ($tagObject.object.sha -ne $Expected) {
        throw "Source tag resolves to '$($tagObject.object.sha)', not expected commit '$Expected'."
    }
    return $Expected
}

function Write-QuickstartNupkg {
    param(
        [Parameter(Mandatory)][string] $Stage,
        [Parameter(Mandatory)][string] $PackageId,
        [Parameter(Mandatory)][string] $Version,
        [Parameter(Mandatory)][string] $LicensePath,
        [Parameter(Mandatory)][string] $PackagePath
    )

    $idXml = [Security.SecurityElement]::Escape($PackageId)
    $versionXml = [Security.SecurityElement]::Escape($Version)
    $licenseXml = [Security.SecurityElement]::Escape($LicensePath)
    $nuspecPath = Join-Path $Stage "$PackageId.nuspec"
    $nuspec = @"
<?xml version="1.0" encoding="utf-8"?>
<package xmlns="http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd">
  <metadata>
    <id>$idXml</id>
    <version>$versionXml</version>
    <authors>Azure Functions Team</authors>
    <description>Azure Functions quickstart project template.</description>
    <license type="file">$licenseXml</license>
    <packageTypes><packageType name="Template" /></packageTypes>
  </metadata>
</package>
"@
    [IO.File]::WriteAllText($nuspecPath, $nuspec.Replace("`r`n", "`n"), [Text.UTF8Encoding]::new($false))

    $files = @(Get-ChildItem -LiteralPath $Stage -File -Recurse |
        Where-Object { $_.FullName -notin @($PackagePath) })
    $files = @(Get-OrdinallySortedFiles -Files $files -Root $Stage)
    $stream = [IO.File]::Create($PackagePath)
    try {
        $archive = [System.IO.Compression.ZipArchive]::new($stream, [System.IO.Compression.ZipArchiveMode]::Create, $false)
        try {
            foreach ($file in $files) {
                $relative = $file.FullName.Substring($Stage.Length).TrimStart('\', '/').Replace('\', '/')
                $entryName = $relative
                $entry = $archive.CreateEntry($entryName, [System.IO.Compression.CompressionLevel]::NoCompression)
                $entry.LastWriteTime = [DateTimeOffset]::new(1980, 1, 1, 0, 0, 0, [TimeSpan]::Zero)
                $input = [IO.File]::OpenRead($file.FullName)
                try {
                    $output = $entry.Open()
                    try { $input.CopyTo($output) } finally { $output.Dispose() }
                } finally { $input.Dispose() }
            }
        } finally { $archive.Dispose() }
    } finally { $stream.Dispose() }
}

function Get-OrdinallySortedFiles {
    param(
        [Parameter(Mandatory)][System.IO.FileInfo[]] $Files,
        [Parameter(Mandatory)][string] $Root
    )

    $sorted = [System.Collections.Generic.SortedDictionary[string, System.IO.FileInfo]]::new(
        [StringComparer]::Ordinal
    )
    foreach ($file in $Files) {
        $relative = $file.FullName.Substring($Root.Length).TrimStart('\', '/').Replace('\', '/')
        if ($sorted.ContainsKey($relative)) {
            throw "Package contains duplicate normalized path '$relative'."
        }
        $sorted.Add($relative, $file)
    }
    return @($sorted.Values)
}

function Get-QuickstartFilesManifest {
    param(
        [Parameter(Mandatory)][string] $ProjectRoot,
        [string] $ExcludePath
    )

    $files = @(Get-ChildItem -LiteralPath $ProjectRoot -File -Recurse |
        Where-Object { -not $ExcludePath -or $_.FullName -ne $ExcludePath })
    $files = @(Get-OrdinallySortedFiles -Files $files -Root $ProjectRoot)
    return @($files | ForEach-Object {
        [ordered]@{
            path   = $_.FullName.Substring($ProjectRoot.Length).TrimStart('\', '/').Replace('\', '/')
            length = $_.Length
            sha256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash.ToLowerInvariant()
        }
    })
}

function Invoke-QuickstartPack {
    param(
        [string] $Id,
        [string] $Version,
        [string] $Expected,
        [string] $Ref,
        [string] $Feed,
        [string] $OutPath
    )

    $versionPattern = '^(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)(-(0|[1-9][0-9]*|[0-9A-Za-z-]*[A-Za-z-][0-9A-Za-z-]*)(\.(0|[1-9][0-9]*|[0-9A-Za-z-]*[A-Za-z-][0-9A-Za-z-]*))*)?$'
    if ($Version -notmatch $versionPattern) {
        throw "Package version '$Version' must be an exact normalized NuGet version without build metadata."
    }
    $feedUri = $null
    if (-not [Uri]::TryCreate($Feed, [UriKind]::Absolute, [ref]$feedUri) -or
        $feedUri.Scheme -ne 'https' -or $feedUri.UserInfo -or $feedUri.Query -or $feedUri.Fragment) {
        throw 'Package source must be an absolute HTTPS feed URL without credentials, query, or fragment.'
    }
    $root = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..\'))
    $manifestPath = Join-Path $root 'Functions.Templates\Template-Manifest\manifest.json'
    $schemaPath = Join-Path $root 'Functions.Templates\Template-Manifest\schemas\manifest.schema.json'
    $manifestJson = Get-Content -LiteralPath $manifestPath -Raw
    if (-not (Test-Json -Json $manifestJson -SchemaFile $schemaPath)) {
        throw 'The current manifest does not validate against manifest.schema.json.'
    }
    $entry = Get-QuickstartManifestEntry -ManifestPath $manifestPath -Id $Id
    if (-not $Ref) { $Ref = [string]$entry.gitRef }
    if ($entry.repositoryUrl -notmatch '^https://github\.com/([^/]+)/([^/]+)/?$') {
        throw "Manifest repositoryUrl '$($entry.repositoryUrl)' is not a GitHub repository URL."
    }
    $repository = "$($Matches[1])/$($Matches[2])"
    $packageId = Get-QuickstartPackageId -Id $Id
    $identity = "$packageId.Project"

    if ($entry.packageReference) {
        if ($entry.packageReference.packageId -cne $packageId -or
            $entry.packageReference.templateIdentity -cne $identity -or
            $entry.packageReference.source -cne $Feed) {
            throw "Existing package ID, template identity, or feed for '$Id' is immutable. Resolve a reviewed migration before packaging."
        }
    }
    foreach ($other in $manifest.templates) {
        if ($other.id -ceq $Id) { continue }
        if ($other.packageReference -and
            ($other.packageReference.packageId -ieq $packageId -or $other.packageReference.templateIdentity -ceq $identity)) {
            throw "Package ID or template identity '$packageId' is already assigned to '$($other.id)'."
        }
    }
    $commit = Get-QuickstartPinnedCommit -Repository $repository -Ref $Ref -Expected $Expected
    $releaseDirectory = Join-Path ([IO.Path]::GetFullPath($OutPath)) "$Id\$Version"
    $outputRoot = Split-Path -Parent $releaseDirectory
    $null = New-Item -ItemType Directory -Path $outputRoot -Force
    $stage = Join-Path $outputRoot ".staging-$([Guid]::NewGuid().ToString('N'))"
    $null = New-Item -ItemType Directory -Path $stage
    $script:QuickstartLicensePath = $null
    $archivePath = Join-Path $stage 'source.zip'

    try {
        $archiveUri = "https://api.github.com/repos/$repository/zipball/$commit"
        Invoke-WebRequest -Uri $archiveUri -OutFile $archivePath `
            -Headers @{ Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' } `
            -MaximumRedirection 5
        Add-Type -AssemblyName System.IO.Compression
        $archive = [System.IO.Compression.ZipFile]::OpenRead($archivePath)
        try {
            $archiveRoot = Get-QuickstartArchiveRoot -Archive $archive
            $projectRoot = Join-Path $stage 'content\quickstart'
            $legalRoot = Join-Path $stage 'license'
            $null = New-Item -ItemType Directory -Path $projectRoot -Force
            Copy-QuickstartProjectContent -Archive $archive -ArchiveRoot $archiveRoot `
                -FolderPath ([string]$entry.folderPath) -Destination $projectRoot -LegalDestination $legalRoot
        } finally { $archive.Dispose() }
        Remove-Item -LiteralPath $archivePath -Force

        $config = Get-QuickstartTemplateConfig -PackageId $packageId -TemplateId $Id `
            -DisplayName ([string]$entry.displayName) -Language ([string]$entry.language)
        $configPath = Join-Path $projectRoot '.template.config\template.json'
        $null = New-Item -ItemType Directory -Path (Split-Path -Parent $configPath) -Force
        $configJson = $config | ConvertTo-Json -Depth 5
        [IO.File]::WriteAllText($configPath, $configJson.Replace("`r`n", "`n") + "`n", [Text.UTF8Encoding]::new($false))

        $packagePath = Join-Path $stage "$packageId.$Version.nupkg"
        Write-QuickstartNupkg -Stage $stage -PackageId $packageId -Version $Version `
            -LicensePath $script:QuickstartLicensePath -PackagePath $packagePath
        $packageHash = (Get-FileHash -LiteralPath $packagePath -Algorithm SHA256).Hash.ToLowerInvariant()
        $inventory = Get-QuickstartFilesManifest -ProjectRoot $stage -ExcludePath $packagePath

        $release = [ordered]@{
            schemaVersion    = 1
            templateId       = $Id
            packageId        = $packageId
            version          = $Version
            sourceRepository = $entry.repositoryUrl
            sourceRef        = $Ref
            sourceCommit     = $commit
            sourceFolder     = $entry.folderPath
            packageSource    = $Feed
            templateIdentity = $identity
            packageSha256    = $packageHash
            contentRevision  = $commit
            templates        = @(@{
                path        = 'content/quickstart/.template.config/template.json'
                identity    = $identity
                type        = 'project'
                contentRole = 'additional-project'
            })
            files            = $inventory
            packagePublished = $false
            qualification    = 'Not approved. Requires independent CLI stack and artifact review.'
        }
        $releaseJson = $release | ConvertTo-Json -Depth 12
        [IO.File]::WriteAllText((Join-Path $stage 'release.json'), $releaseJson.Replace("`r`n", "`n") + "`n", [Text.UTF8Encoding]::new($false))
        $checksumText = "$packageHash  $packageId.$Version.nupkg`n"
        [IO.File]::WriteAllText((Join-Path $stage 'checksums.sha256'), $checksumText, [Text.UTF8Encoding]::new($false))

        $candidate = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json -AsHashtable
        $candidateEntry = @($candidate.templates | Where-Object { $_.id -ceq $Id })[0]
        $manifestVersion = [version]$candidate.version
        $candidate.version = "$($manifestVersion.Major).$($manifestVersion.Minor).$($manifestVersion.Build + 1)"
        $candidate.generatedAt = [DateTime]::UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'")
        $candidateEntry.packageReference = [ordered]@{
            schemaVersion    = 1
            packageId        = $packageId
            version          = $Version
            source           = $Feed
            templateIdentity = $identity
            artifact         = [ordered]@{
                algorithm         = 'sha256'
                digest            = $packageHash
                evidenceReference = "$Id/$Version/release.json"
            }
        }
        $candidateEntry.gitRef = $Ref
        $candidateJson = $candidate | ConvertTo-Json -Depth 100
        $candidatePath = Join-Path $stage 'manifest.next.json'
        [IO.File]::WriteAllText($candidatePath, $candidateJson.Replace("`r`n", "`n") + "`n", [Text.UTF8Encoding]::new($false))
        if (-not (Test-Json -Json (Get-Content -LiteralPath $candidatePath -Raw) -SchemaFile $schemaPath)) {
            throw 'Generated candidate manifest does not validate against manifest.schema.json.'
        }

        $inventoryJson = $inventory | ConvertTo-Json -Depth 10
        [IO.File]::WriteAllText((Join-Path $stage 'inventory.json'), $inventoryJson.Replace("`r`n", "`n") + "`n", [Text.UTF8Encoding]::new($false))

        if (Test-Path -LiteralPath $releaseDirectory) {
            $existingPackage = Join-Path $releaseDirectory "$packageId.$Version.nupkg"
            $existingRelease = Join-Path $releaseDirectory 'release.json'
            if ((Test-Path $existingPackage) -and (Test-Path $existingRelease) -and
                (Get-FileHash -LiteralPath $existingPackage -Algorithm SHA256).Hash -eq
                (Get-FileHash -LiteralPath $packagePath -Algorithm SHA256).Hash -and
                (Get-Content -LiteralPath $existingRelease -Raw | ConvertFrom-Json | Where-Object {
                    $_.sourceCommit -eq $commit -and $_.sourceRef -eq $Ref -and
                    $_.sourceRepository -eq $entry.repositoryUrl -and $_.sourceFolder -eq $entry.folderPath -and
                    $_.packageSource -eq $Feed -and $_.templateIdentity -ceq $identity
                })) {
                Remove-Item -LiteralPath $stage -Recurse -Force
                Write-Host "No-op: identical package already exists at '$releaseDirectory'."
                return
            }
            throw "Release '$Id' $Version already exists with different content or provenance. Choose a new version."
        }

        [IO.Directory]::Move($stage, $releaseDirectory)
        Write-Host "Created local package at '$releaseDirectory'."
        Write-Host "Package SHA256: $packageHash"
        Write-Host 'Not published or CLI-qualified. Review manifest.next.json and release.json before any separate release approval.'
    }
    catch {
        if (Test-Path -LiteralPath $stage) {
            Remove-Item -LiteralPath $stage -Recurse -Force
        }
        throw
    }
}

if (-not $script:QuickstartPackagerDotSourced) {
    Invoke-QuickstartPack -Id $TemplateId -Version $PackageVersion -Expected $ExpectedCommit `
        -Ref $SourceRef -Feed $PackageSource -OutPath $OutputPath
}
