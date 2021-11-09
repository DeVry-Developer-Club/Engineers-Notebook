Param(
    [Parameter(Mandatory=$false, Position=0)]
    $tag
)

# if value is not provided we'll mark the tag as test
if(!$tag) { $tag="test" }


# This script lives within the root directory of our project
# we want to make it so we can invoke this script from any
# location and still work

$mypath = $MyInvocation.MyCommand.Path
$projectDir = Split-Path $mypath -Parent

# Pathing Variables for our projects
$publicApiDocker = Join-Path $projectDir "EngineerNotebook.PublicApi/Dockerfile"
$blazorDocker = Join-Path $projectDir "EngineerNotebook.BlazorAdmin/Dockerfile"


Write-Host "Project Directories"
Write-Host "     Public API: $publicApiDocker"
Write-Host "     Blazor: $blazorDocker"

Write-Host "Creating Docker Image for Public API...."
docker build -t "engineer-notebook-public-api:$tag" -f "$publicApiDocker" .

Write-Host "Creating Docker Image for Blazor Admin..."
docker build -t "engineer-notebook-blazor-admin:$tag" -f "$blazorDocker" .

Write-Host "Build Complete..."