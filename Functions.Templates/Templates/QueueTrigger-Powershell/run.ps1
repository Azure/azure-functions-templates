$in = Get-Content $triggerInput -Raw
Write-Output "PowerShell script processed queue message '$in'"
