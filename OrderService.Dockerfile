FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app

# Installed icu libs for global cultures (missing in alpine)
RUN apk add icu
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR "/src"
COPY ["Directory.Build.props", ""]
COPY ["order-service/OrderService.Api/OrderService.Api.csproj", "order-service/OrderService.Api/"]
COPY ["order-service/OrderService.Application/OrderService.Application.csproj", "order-service/OrderService.Application/"]
COPY ["order-service/OrderService.Domain/OrderService.Domain.csproj", "order-service/OrderService.Domain/"]
COPY ["order-service/OrderService.OrderManager/OrderService.OrderManager.csproj", "order-service/OrderService.OrderManager/"]
COPY ["order-service/OrderService.Persistence/OrderService.Persistence.csproj", "order-service/OrderService.Persistence/"]
COPY ["libraries/Libraries.Common/Libraries.Common.csproj", "libraries/Libraries.Common/"]

RUN dotnet restore "order-service/OrderService.Api/OrderService.Api.csproj"
COPY . .
WORKDIR "/src/order-service/OrderService.Api"
RUN dotnet build "OrderService.Api.csproj" -c Release -o "/app/build"

FROM build AS publish
WORKDIR "/src/order-service/OrderService.Api"
RUN dotnet publish "OrderService.Api.csproj" -c Release --no-restore -o "/app/publish"

FROM base AS final
WORKDIR "/app"
COPY --from=publish "/app/publish" .

ENTRYPOINT ["dotnet", "OrderService.Api.dll"]
