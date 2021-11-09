# The intent of this script is to generate 
# the SQL scripts that are necessary to ensure our production database is properly 
# created/synced

# Unfortunately EF doesn't work with blank database, so it requires the Script approach....

$mypath = $MyInvocation.MyCommand.Path
$projectDir = Split-Path $mypath -Parent

# Pathing Variables for our projects
$api = Join-Path $projectDir "EngineerNotebook.PublicApi"

$identityContext = "AppIdentityDbContext"
$engineerContext = "EngineerDbContext"
$output = Join-Path $projectDir "sql"

Write-Host "Updating Migration Scripts for production use"
dotnet ef migrations script -i --project "EngineerNotebook.PublicApi" --context $engineerContext -o "$output/engineer.sql"
dotnet ef migrations script -i --project "EngineerNotebook.PublicApi" --context $identityContext -o "$output/identity.sql"

Write-Host "Completed SQL Script Generation..."