#!/usr/bin/env bash
set -eux
export $(grep -v '^#' .env | xargs)

yc serverless function create --name=$FUNCTION_NAME || echo "Function already exists"
if [[ ! -e "build" ]]; then
    mkdir "build"
fi

cp *.cs ./build
cp *.csproj ./build
#dotnet publish

yc serverless function version create \
  --function-name=$FUNCTION_NAME \
  --runtime dotnetcore31-preview \
  --entrypoint Handler \
  --memory 128m \
  --execution-timeout 30s \
  --source-path ./build\
  --environment AWS_ACCESS_KEY=$AWS_ACCESS_KEY,AWS_SECRET_KEY=$AWS_SECRET_KEY,ENDPOINT=$ENDPOINT


rm -rf ./build
