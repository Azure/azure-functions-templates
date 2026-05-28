<#
.SYNOPSIS
    Validates manifest template references: verifies each template's repo, gitRef, and folderPath exist on GitHub.

.DESCRIPTION
    For each unique (repositoryUrl, gitRef, folderPath) tuple in the manifest:
    1. Verifies the repository is accessible.
    2. Verifies the gitRef is fully qualified (refs/tags/* or refs/heads/*) and resolves on GitHub.
    3. Verifies the folderPath exists at that ref (skipped for '.').
    Warns when gitRef is a branch (refs/heads/*) instead of a signed tag.

    Requires: gh CLI authenticated with repo read access.

.PARAMETER ManifestPath
    Path to the manifest.json file to validate.

.EXAMPLE
    .\validate-manifest-refs.ps1
    .\validate-manifest-refs.ps1 -ManifestPath ./manifest.json
#>

param(
    [string]$ManifestPath
)

$ErrorActionPreference = "Stop"

if (-not $ManifestPath) {
    $ManifestPath = Join-Path $PSScriptRoot "../../Functions.Templates/Template-Manifest/manifest.json"
}

if (-not (Test-Path $ManifestPath)) {
    Write-Host "ERROR: Manifest not found at $ManifestPath" -ForegroundColor Red
    exit 1
}

# Verify gh is available
$null = Get-Command gh -ErrorAction Stop

$manifest = Get-Content $ManifestPath -Raw | ConvertFrom-Json
$errors = @()
$warnings = @()
$checked = @{}
$passed = 0

Write-Host "`n=== Manifest Smoke Test ===" -ForegroundColor Cyan
Write-Host "File: $ManifestPath"
Write-Host "Checking $($manifest.templates.Count) templates...`n"

foreach ($t in $manifest.templates) {
    $repoKey = "$($t.repositoryUrl)|$($t.gitRef)|$($t.folderPath)"
    if ($checked.ContainsKey($repoKey)) { continue }
    $checked[$repoKey] = $true

    $repoPath = $t.repositoryUrl -replace 'https://github.com/', ''
    $label = "$repoPath@$($t.gitRef)"

    # Check repo exists
    $null = gh api "repos/$repoPath" --jq '.full_name' 2>&1
    if ($LASTEXITCODE -ne 0) {
        $errors += "$($t.id): repo not accessible — $repoPath"
        Write-Host "  FAIL  $label (repo not found)" -ForegroundColor Red
        continue
    }

    # Check gitRef exists — must be fully qualified (refs/tags/* or refs/heads/*)
    $gitRef = $t.gitRef
    if ($gitRef -notmatch '^refs/(tags|heads)/') {
        $errors += "$($t.id): gitRef '$gitRef' is not fully qualified — must start with refs/tags/ or refs/heads/"
        Write-Host "  FAIL  $label (gitRef not fully qualified)" -ForegroundColor Red
        continue
    }

    $null = gh api "repos/$repoPath/git/$gitRef" --jq '.ref' 2>&1
    if ($LASTEXITCODE -ne 0) {
        $errors += "$($t.id): gitRef '$gitRef' not found in $repoPath"
        Write-Host "  FAIL  $label (ref not found)" -ForegroundColor Red
        continue
    } elseif ($gitRef -match '^refs/heads/') {
        $warnings += "$($t.id): '$gitRef' is a branch, not a signed tag — $repoPath"
    }

    # Check folderPath exists at the gitRef
    $contentPath = if ($t.folderPath -eq '.') { '' } else { "/$($t.folderPath)" }
    $null = gh api "repos/$repoPath/contents${contentPath}?ref=$($t.gitRef)" --jq '.[0].name // .name' 2>&1
    if ($LASTEXITCODE -ne 0) {
        $errors += "$($t.id): folderPath '$($t.folderPath)' not found at ref '$($t.gitRef)' in $repoPath"
        Write-Host "  FAIL  $label (folder not found at ref)" -ForegroundColor Red
        continue
    }

    $passed++
    Write-Host "  OK    $label" -ForegroundColor Green
}

# Summary
Write-Host "`n=== Summary ===" -ForegroundColor Cyan
Write-Host "Checked: $($checked.Count) unique repo+ref+path combinations"
Write-Host "Passed:  $passed" -ForegroundColor Green

if ($warnings.Count -gt 0) {
    Write-Host "Warnings: $($warnings.Count)" -ForegroundColor Yellow
    foreach ($w in $warnings) { Write-Host "  WARN: $w" -ForegroundColor Yellow }
}

if ($errors.Count -gt 0) {
    Write-Host "Errors: $($errors.Count)" -ForegroundColor Red
    foreach ($e in $errors) { Write-Host "  ERROR: $e" -ForegroundColor Red }
    exit 1
} else {
    Write-Host "`nMANIFEST REFS VALIDATION PASSED" -ForegroundColor Green
    exit 0
}
