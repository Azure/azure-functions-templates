# Load NuGet.Versioning assembly
Add-Type -Path "C:\Program Files\PackageManagement\NuGet\Packages\NuGet.Versioning.6.13.2\lib\netstandard2.0\NuGet.Versioning.dll"

# Define the target ActionId
$targetActionId = "B17581D1-C5C9-4489-8F0A-004BE667B814"

# Get all template.json files in the current directory and subdirectories
$templateFiles = Get-ChildItem -Path "..\Functions.Templates\Templates" -Recurse -Filter "template.json"

foreach ($file in $templateFiles) {
    # Read the content of the file
    $content = Get-Content -Path $file.FullName -Raw | ConvertFrom-Json
    $originalContent = $content | ConvertTo-Json -Depth 10

    # Check if the postActions array contains the target ActionId
    foreach ($postAction in $content.postActions) {
        if ($postAction.ActionId -eq $targetActionId) {
            # Fetch the reference (package ID)
            $packageId = $postAction.args.reference
            $currentVersion = $postAction.args.version
            Write-Host "File: $($file.FullName)"
            Write-Host "Package ID: $packageId"
            Write-Host "Current Version: $currentVersion"

            # Fetch the latest version of the package from NuGet.org
            $nugetUrl = "https://api.nuget.org/v3/registration5-gz-semver2/$packageId/index.json"
            $nugetUrl = $nugetUrl.ToLower()
            Write-Host "Fetching latest version from $nugetUrl"

            try {
                $response = Invoke-RestMethod -Uri $nugetUrl -Method Get
                if ($response.items) {
                    # Flatten all listed versions from the registration data
                    $allVersions = @()
                    foreach ($item in $response.items) {
                        foreach ($page in $item.items) {
                            if ($page.catalogEntry.listed -eq $true) {
                                $allVersions += $page.catalogEntry.version
                            }
                        }
                    }

                    if ($allVersions) {
                        # Parse the current major version using NuGet.Versioning
                        $currentMajorVersion = [NuGet.Versioning.NuGetVersion]::Parse($currentVersion).Major

                        # Filter versions within the same major version
                        $filteredVersions = $allVersions | Where-Object {
                            ([NuGet.Versioning.NuGetVersion]::Parse($_).Major -eq $currentMajorVersion)
                        }

                        if ($filteredVersions) {
                            # Get the latest version within the same major version
                            $latestVersion = $filteredVersions | Sort-Object { [NuGet.Versioning.NuGetVersion]::Parse($_) } | Select-Object -Last 1
                            Write-Host "Latest version of $packageId within major version $currentMajorVersion is $latestVersion"

                            # Check if the current version is already the latest
                            if ($currentVersion -eq $latestVersion) {
                                Write-Host "The current version ($currentVersion) is already the latest. No update needed."
                            } else {
                                # Update the version in the template.json file
                                $postAction.args.version = $latestVersion
                                Write-Host "Updated version in $($file.FullName) to $latestVersion"
                            }
                        } else {
                            Write-Host "No listed versions found for $packageId within major version $currentMajorVersion"
                        }
                    } else {
                        Write-Host "No listed versions found for $packageId on NuGet.org"
                    }
                } else {
                    Write-Host "No versions found for $packageId on NuGet.org"
                }
            } catch {
                Write-Host "Error: $($_.Exception.Message)"
                Write-Host "Failed to fetch the latest version for $packageId from NuGet.org"
            }
        }
    }

    # Write the updated content back to the file only if changes were made
    $updatedContent = $content | ConvertTo-Json -Depth 10
    if ($originalContent -ne $updatedContent) {
        $updatedContent | Set-Content -Path $file.FullName
        Write-Host "File updated: $($file.FullName)"
    } else {
        Write-Host "No changes detected for file: $($file.FullName)"
    }
}