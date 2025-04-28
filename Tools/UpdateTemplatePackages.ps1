# PowerShell script to update package versions in template.json files

# Define the target ActionId
$targetActionId = "B17581D1-C5C9-4489-8F0A-004BE667B814"

# Get all template.json files in the current directory and subdirectories
$templateFiles = Get-ChildItem -Path "..\Functions.Templates\Templates" -Recurse -Filter "template.json"

foreach ($file in $templateFiles) {
    # Read the content of the file
    $content = Get-Content -Path $file.FullName -Raw | ConvertFrom-Json

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
            $nugetUrl = "https://api.nuget.org/v3-flatcontainer/$packageId/index.json"
            $nugetUrl = $nugetUrl.ToLower()
            Write-Host "Fetching latest version from $nugetUrl"

            try {
                $response = Invoke-RestMethod -Uri $nugetUrl -Method Get
                if ($response.versions) {
                    # Parse the current major version
                    $currentMajorVersion = [Version]$currentVersion.Major

                    # Filter versions within the same major version
                    $filteredVersions = $response.versions | Where-Object {
                        ([Version]$_).Major -eq $currentMajorVersion
                    }

                    if ($filteredVersions) {
                        # Get the latest version within the same major version
                        $latestVersion = $filteredVersions[-1]
                        Write-Host "Latest version of $packageId within major version $currentMajorVersion is $latestVersion"

                        # Update the version in the template.json file
                        $postAction.args.version = $latestVersion
                        Write-Host "Updated version in $($file.FullName) to $latestVersion"
                    } else {
                        Write-Host "No versions found for $packageId within major version $currentMajorVersion"
                    }
                } else {
                    Write-Host "No versions found for $packageId on NuGet.org"
                }
            } catch {
                Write-Host "Failed to fetch the latest version for $packageId from NuGet.org"
            }
        }
    }

    # Write the updated content back to the file
    $content | ConvertTo-Json -Depth 10 | Set-Content -Path $file.FullName
}