$fileMap = [ordered]@{
    "cs" = "cs-CZ"
    "de" = "de-DE"
    "es" = "es-ES"
    "fr" = "fr-FR"
    "hu" = "hu-HU"
    "it" = "it-IT"
    "ja" = "ja-JP"
    "ko" = "ko-KR"
    "nl" = "nl-NL"
    "pl" = "pl-PL"
    "pt-BR" = "pt-BR"
    "pt-PT" = "pt-PT"
    "ru" = "ru-RU"
    "sv" = "sv-SE"
    "tr" = "tr-TR"
    "zh-Hans" = "zh-CN"
    "zh-Hant" = "zh-TW"
}

$sourceBasePath = Join-Path -Path $env:localPath -ChildPath "locTemp"
$destinationBasePath = Join-Path -Path $env:localPath -ChildPath "Functions.Templates\Resources"

foreach ($key in $fileMap.Keys) {
    $sourceDirectory = Join-Path -Path $sourceBasePath -ChildPath $key
    $sourceFile = Join-Path -Path $sourceDirectory -ChildPath "Resources.resx"

    $destinationParentDirectory = Join-Path -Path $destinationBasePath -ChildPath $fileMap[$key]
    $destinationDirectory = Join-Path -Path $destinationParentDirectory -ChildPath "Resources"

    Write-Host $sourceFile $destinationDirectory
    Copy-Item -Path $sourceFile -Destination $destinationDirectory -Force 
}