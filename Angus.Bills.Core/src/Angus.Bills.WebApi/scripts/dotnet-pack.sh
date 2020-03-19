#!/bin/bash
echo Executing after success scripts on branch $TRAVIS_BRANCH
echo Triggering Nuget package build

cd src/Angus.Bills.WebApi/src/Angus.Bills.WebApi
dotnet pack -c release /p:PackageVersion=0.4.$TRAVIS_BUILD_NUMBER --no-restore -o .

echo Uploading Angus.Bills.WebApi package to Nuget using branch $TRAVIS_BRANCH

case "$TRAVIS_BRANCH" in
  "master")
    dotnet nuget push *.nupkg -k $NUGET_API_KEY -s https://api.nuget.org/v3/index.json
    ;;
esac