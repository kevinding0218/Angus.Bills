#!/bin/bash
echo Executing after success scripts on branch $TRAVIS_BRANCH
echo Triggering Nuget package build

cd src/Drex.Logging.CQRS/src/Drex.Logging.CQRS
dotnet pack -c release /p:PackageVersion=0.4.0 --no-restore -o ./../../../../release

#echo Uploading Drex.Logging.CQRS package to Nuget using branch $TRAVIS_BRANCH

# case "$TRAVIS_BRANCH" in
#   "master")
#     dotnet nuget push *.nupkg -k oy2oufdkr64me6dbxgeabvkk3kl6vpwunfzjtxpoxjuey4 -s https://api.nuget.org/v3/index.json
#     ;;
# esac