#!/usr/bin/env bash

PROJECT_ROOT=$(pwd)
API_DIR="$PROJECT_ROOT/EngineerNotebook.PublicApi"
BLAZOR_DIR="$PROJECT_ROOT/EngineerNotebook.BlazorAdmin"

 TAG="test"

 while getopts t: flag
 do
	case "${flag}" in
		u)
			TAG=${OPTARG}		
		;;
    *)
      ;;
	esac
 done
 
echo "'$TAG' shall be applied to the created images

    EngineerNotebook-Public-Api:$TAG
    EngineerNotebook-Blazor-Admin:$TAG

"

echo "Building Public API..."
docker build -f $API_DIR/Dockerfile -t EngineersNotebook-Public-Api:$TAG .

echo "Completed Building Public API..."

echo "Building Blazor Admin..."
docker build -f $BLAZOR_DIR/Dockerfile -t EngineerNotebook-Blazor-Admin:$TAG .


echo "The following images were built

    EngineerNotebook-Public-Api:$TAG
    EngineerNotebook-Blazor-Admin:$TAG

"