#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AssetRental.API/AssetRental.API.csproj", "AssetRental.API/"]
COPY ["AssetRental.Application/AssetRental.Application.csproj", "AssetRental.Application/"]
COPY ["AssetRental.Domain/AssetRental.Domain.csproj", "AssetRental.Domain/"]
COPY ["AssetRental.Infrastructure/AssetRental.Infrastructure.csproj", "AssetRental.Infrastructure/"]
RUN dotnet restore "./AssetRental.API/AssetRental.API.csproj"
COPY . .
WORKDIR "/src/AssetRental.API"
RUN dotnet build "./AssetRental.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AssetRental.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AssetRental.API.dll"]