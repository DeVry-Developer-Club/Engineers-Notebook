Param(
    [Parameter(Mandatory=$false, Position=0)]
    $tag,

    [Parameter(Mandatory=$false, Position=1)]
    $sslPassword,

    [Parameter(Mandatory=$false, Position=2)]
    $databaseUsername,

    [Parameter(Mandatory=$false, Position=3)]
    $databasePassword
)

# if value is not provided we'll mark the tag as test
if(!$tag) { $tag="test" }
if(!$sslPassword) { $sslPassword="devry123"}
if(!$databasePassword) { $databasePassword="devry123"}
if(!$databaseUsername) { $databaseUsername="root"}

# This script lives within the root directory of our project
# we want to make it so we can invoke this script from any
# location and still work

$mypath = $MyInvocation.MyCommand.Path
$projectDir = Split-Path $mypath -Parent

# create the artifact destination for the tar files
$outputDir = Join-Path $projectDir "deployment-artifacts"
Remove-Item -Recurse -Force $outputDir
mkdir -p $outputDir

# We need to output our image as a tar so we can transfer it to other systems
Write-Host "Saving Public API Image as tar..."
$publicApiTar = Join-Path $outputDir "engineers-notebook-public-api.tar"
docker save -o $publicApiTar "engineer-notebook-public-api:$tag"

# The appsettings.json file that will be shipped with the docker-compose file
Write-Host "Copying appsettings.json file..."
$appsettingsFile = Join-Path $projectDir "EngineerNotebook.PublicApi/appsettings.json"
$appsettingsFileDest = Join-Path $outputDir "appsettings.json"
Copy-Item $appsettingsFile $appsettingsFileDest

# the docker-compose file that is required to run all services together
# this file does require modification in order to work in a linux environment
Write-Host "Copying docker-compose.yml file..."
$dockerCompose = Join-Path $projectDir "docker-compose-linux.yml"
$dockerComposeDest = Join-Path $outputDir "docker-compose.yml"
Copy-Item $dockerCompose $dockerComposeDest

# this file is required to ensure our mongo database is initilized on first-startup
Write-Host "Copying initmongo.js file..."
$initmongo = Join-Path $projectDir "init-mongo.js"
$initmongoDest = Join-Path $outputDir "init-mongo.js"
Copy-Item $initmongo $initmongoDest

# create environment file that shall be used for default settings
$envFile = Join-Path $outputDir ".env"
New-Item $envFile -ItemType File
Add-Content $envFile ("TAG=" + $tag)
Add-Content $envFile ("SSL_PASSWORD=" + $sslPassword)
Add-Content $envFile ("DATABASE_USERNAME=" + $databaseUsername)
Add-Content $envFile ("DATABASE_PASSWORD=" + $databasePassword)

$scriptFile = Join-Path $outputDir "run-engineer-notebook.sh"
New-Item $scriptFile -ItemType File
Add-Content $scriptFile "#!/bin/bash"
Add-Content $scriptFile 'set CURRENT_DIR=$(pwd)'
Add-Content $scriptFile "docker-compose up -d"

Write-Host "Deployment artifacts ready..."