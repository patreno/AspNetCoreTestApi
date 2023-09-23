FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-env

#See: https://pumpingco.de/blog/how-to-run-a-sonarcloud-scan-during-docker-builds-for-dotnet-core/

RUN apk add openjdk17
RUN dotnet tool install --global dotnet-sonarscanner

ARG SONAR_PROJECT_KEY
ARG SONAR_TOKEN
ARG SONAR_OGRANIZAION_KEY
ARG SONAR_HOST_URL=https://sonarcloud.io

WORKDIR /App

# Copy everything
COPY . ./

ENV PATH="${PATH}:/root/.dotnet/tools"

# Restore as distinct layers
RUN dotnet restore

RUN dotnet sonarscanner begin /o:$SONAR_OGRANIZAION_KEY /k:$SONAR_PROJECT_KEY /d:sonar.host.url=$SONAR_HOST_URL /d:sonar.login=$SONAR_TOKEN

RUN dotnet build -c Release

RUN dotnet sonarscanner end /d:sonar.login=$SONAR_TOKEN

# Build and publish a release
RUN dotnet publish -c Release -o out


# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "AspNetCoreTest.dll"]