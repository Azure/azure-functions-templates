$in = Get-Content $Env:input

[Console]::WriteLine("Powershell script processed queue message '$in'")