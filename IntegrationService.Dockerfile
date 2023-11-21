FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app

# Installed icu libs for global cultures (missing in alpine)
RUN apk add icu
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR "/src"
COPY ["Directory.Build.props", ""]
COPY ["integration-service/IntegrationService.Worker/IntegrationService.Worker.csproj", "integration-service/IntegrationService.Worker/"]
COPY ["integration-service/IntegrationService.AgvProviderFactoryHu/IntegrationService.AgvProviderFactoryHu.csproj", "integration-service/IntegrationService.AgvProviderFactoryHu/"]
COPY ["integration-service/IntegrationService.AgvProviderFactoryUs/IntegrationService.AgvProviderFactoryUs.csproj", "integration-service/IntegrationService.AgvProviderFactoryUs/"]
COPY ["libraries/Libraries.Common/Libraries.Common.csproj", "libraries/Libraries.Common/"]

RUN dotnet restore "integration-service/IntegrationService.Worker/IntegrationService.Worker.csproj"
COPY . .
WORKDIR "/src/integration-service/IntegrationService.Worker"
RUN dotnet build "IntegrationService.Worker.csproj" -c Release -o "/app/build"

FROM build AS publish
WORKDIR "/src/integration-service/IntegrationService.Worker"
RUN dotnet publish "IntegrationService.Worker.csproj" -c Release --no-restore -o "/app/publish"

FROM base AS final
WORKDIR "/app"
COPY --from=publish "/app/publish" .

ENTRYPOINT ["dotnet", "IntegrationService.Worker.dll"]
