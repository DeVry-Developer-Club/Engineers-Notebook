# This is meant to get your local settings in place so you can use the application locally 
# without modifying the appsettings.json file for local needs
$mypath = $MyInvocation.MyCommand.Path
$projectDir = Split-Path $mypath -Parent

Write-Host "Setting Server to localhost..."
dotnet user-secrets set "Database:Host" "localhost"

Write-Host "Setting Server Port to 3306..."
dotnet user-secrets set "Database:Port" "27017"

Write-Host "Local database development setup..."
Write-Host "If your local username and password are different please use the `dotnet use-secrets set` for updating accordingly"

Write-Host "Database:Username | Database:Password"