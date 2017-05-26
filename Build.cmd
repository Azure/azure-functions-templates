@echo off

IF NOT "%2" == "" (
    powershell -ExecutionPolicy Bypass -Command %~dpn0.ps1 -target %1 -templateVersion %2
) ELSE IF NOT "%1" == "" (
    powershell -ExecutionPolicy Bypass -Command %~dpn0.ps1 -target %1
) ELSE (
    powershell -ExecutionPolicy Bypass -Command %~dpn0.ps1
)
