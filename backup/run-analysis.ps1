Set-Location "D:\PyFx\azure-functions-templates"

$analysisFile = "backup/build-comparison-analysis.md"
$sb = [System.Text.StringBuilder]::new()

$null = $sb.AppendLine("# Azure Functions Templates Build Comparison Analysis")
$null = $sb.AppendLine("")
$null = $sb.AppendLine("**Date:** $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')")
$null = $sb.AppendLine("**Comparison:** gulp build-all vs build-all.ps1")
$null = $sb.AppendLine("")
$null = $sb.AppendLine("---")
$null = $sb.AppendLine("")

$bundles = "ExtensionBundle.v1.Templates.1.0.1","ExtensionBundle.v2.Templates.2.0.1","ExtensionBundle.v3.Templates.3.0.1","ExtensionBundle.v4.Templates.4.0.1","ExtensionBundle.Preview.v3.Templates.3.0.1","ExtensionBundle.Preview.v4.Templates.4.0.1"

$totalFiles = 0
$identicalFiles = 0
$totalBytes = 0
$bundleResults = @()

foreach ($bundle in $bundles) {
    $gulpPath = "backup/analysis-gulp/$bundle"
    $psPath = "backup/analysis-ps/$bundle"
    
    $bundleTotal = 0
    $bundleIdentical = 0
    $bundleBytes = 0
    $fileResults = @()
    
    $gulpFiles = Get-ChildItem -Path $gulpPath -Recurse -File
    
    foreach ($gf in $gulpFiles) {
        $relativePath = $gf.FullName.Substring((Resolve-Path $gulpPath).Path.Length + 1)
        $bundleTotal++
        $totalFiles++
        $psFilePath = Join-Path $psPath $relativePath
        
        $gulpBytes = [System.IO.File]::ReadAllBytes($gf.FullName)
        $psBytes = [System.IO.File]::ReadAllBytes($psFilePath)
        $bundleBytes += $gulpBytes.Length
        $totalBytes += $gulpBytes.Length
        
        $identical = $true
        if ($gulpBytes.Length -eq $psBytes.Length) {
            for ($i = 0; $i -lt $gulpBytes.Length; $i++) {
                if ($gulpBytes[$i] -ne $psBytes[$i]) {
                    $identical = $false
                    break
                }
            }
        } else {
            $identical = $false
        }
        
        if ($identical) { $bundleIdentical++; $identicalFiles++ }
        
        $fileResults += [PSCustomObject]@{ File = $relativePath; Size = $gulpBytes.Length; Match = $identical }
    }
    
    $bundleResults += [PSCustomObject]@{
        Bundle = $bundle
        TotalFiles = $bundleTotal
        IdenticalFiles = $bundleIdentical
        TotalBytes = $bundleBytes
        FileResults = $fileResults
    }
}

$null = $sb.AppendLine("## Executive Summary")
$null = $sb.AppendLine("")
$null = $sb.AppendLine("| Metric | Value |")
$null = $sb.AppendLine("|--------|-------|")
$null = $sb.AppendLine("| Total Files Compared | **$totalFiles** |")
$null = $sb.AppendLine("| Byte-Identical Files | **$identicalFiles** |")
$null = $sb.AppendLine("| Different Files | **$($totalFiles - $identicalFiles)** |")
$null = $sb.AppendLine("| Total Bytes Analyzed | **$("{0:N0}" -f $totalBytes)** |")
$null = $sb.AppendLine("| Match Rate | **$([math]::Round($identicalFiles / $totalFiles * 100, 2))%** |")
$null = $sb.AppendLine("")

if ($identicalFiles -eq $totalFiles) {
    $null = $sb.AppendLine("> RESULT: All $totalFiles files are BYTE-IDENTICAL between gulp and PowerShell builds")
}

$null = $sb.AppendLine("")
$null = $sb.AppendLine("---")
$null = $sb.AppendLine("")
$null = $sb.AppendLine("## Per-Bundle Summary")
$null = $sb.AppendLine("")
$null = $sb.AppendLine("| Bundle | Files | Identical | Bytes | Status |")
$null = $sb.AppendLine("|--------|-------|-----------|-------|--------|")

foreach ($br in $bundleResults) {
    $status = if ($br.TotalFiles -eq $br.IdenticalFiles) { "MATCH" } else { "DIFF" }
    $null = $sb.AppendLine("| $($br.Bundle) | $($br.TotalFiles) | $($br.IdenticalFiles) | $("{0:N0}" -f $br.TotalBytes) | $status |")
}

$null = $sb.AppendLine("")
$null = $sb.AppendLine("---")
$null = $sb.AppendLine("")
$null = $sb.AppendLine("## Detailed File Analysis")
$null = $sb.AppendLine("")

foreach ($br in $bundleResults) {
    $null = $sb.AppendLine("### $($br.Bundle)")
    $null = $sb.AppendLine("")
    $null = $sb.AppendLine("| File | Size (bytes) | Status |")
    $null = $sb.AppendLine("|------|-------------|--------|")
    
    foreach ($fr in $br.FileResults) {
        $status = if ($fr.Match) { "Identical" } else { "Different" }
        $fileName = $fr.File -replace '\\', '/'
        $null = $sb.AppendLine("| $fileName | $("{0:N0}" -f $fr.Size) | $status |")
    }
    $null = $sb.AppendLine("")
}

$sb.ToString() | Out-File -FilePath $analysisFile -Encoding UTF8
Write-Host "Done: $totalFiles files, $identicalFiles identical"
