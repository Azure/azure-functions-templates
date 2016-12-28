echo OFF
FOR /F "usebackq" %%i IN ('%inputBlob%') DO set size=%%~zi
echo Blob trigger function Processed, blob size:%size% bytes