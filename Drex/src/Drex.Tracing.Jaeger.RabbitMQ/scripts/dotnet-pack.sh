#!/bin/bash
echo Executing after success scripts on branch $TRAVIS_BRANCH
echo Triggering Nuget package build

cd src/Drex.Tracing.Jaeger.RabbitMQ/src/Drex.Tracing.Jaeger.RabbitMQ
dotnet pack -c release /p:PackageVersion=0.4.0 --no-restore --output ./../../../../release

#echo Uploading Drex.Tracing.Jaeger.RabbitMQ package to Nuget using branch $TRAVIS_BRANCH

# case "$TRAVIS_BRANCH" in
#   "master")
#     dotnet nuget push *.nupkg -k oy2oufdkr64me6dbxgeabvkk3kl6vpwunfzjtxpoxjuey4 -s https://api.nuget.org/v3/index.json
#     ;;
# esac