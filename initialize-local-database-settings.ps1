# This is meant to get your local settings in place so you can use the application locally 
# without modifying the appsettings.json file for local needs
$mypath = $MyInvocation.MyCommand.Path
$projectDir = Split-Path $mypath -Parent

Write-Host "Setting Server to localhost..."
dotnet user-secrets set "Databases:Engineer:Server" "localhost"
dotnet user-secrets set "Databases:Identity:Server" "localhost"


Write-Host "Setting Server Port to 3306..."
dotnet user-secrets set "Databases:Engineer:Port" "3306"
dotnet user-secrets set "Databases:Identity:Port" "3306"

Write-Host "Local database development setup..."
Write-Host "If your local username and password are different please use the `dotnet use-secrets set` for updating accordingly"

Write-Host "Databases:Engineer:Username | Databases:Engineer:Password"
Write-Host "Databases:Identity:Username | Databases:Identity:Password"