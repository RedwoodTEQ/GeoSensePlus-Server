#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
# EXPOSE 80
# EXPOSE 443
EXPOSE 5001
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["GeoSensePlus.Server/GeoSensePlus.Server.csproj", "GeoSensePlus.Server/"]
COPY ["GeoSensePlus.App.ProgressTracking/GeoSensePlus.App.ProgressTracking.csproj", "GeoSensePlus.App.ProgressTracking/"]
COPY ["GeoSensePlus.Core/GeoSensePlus.Core.csproj", "GeoSensePlus.Core/"]
COPY ["GeoSensePlus.App.AssetTracking/GeoSensePlus.App.AssetTracking.csproj", "GeoSensePlus.App.AssetTracking/"]
COPY ["GeoSensePlus.Firestore/GeoSensePlus.Firestore.csproj", "GeoSensePlus.Firestore/"]
RUN dotnet restore "GeoSensePlus.Server/GeoSensePlus.Server.csproj"
COPY . .
WORKDIR "/src/GeoSensePlus.Server"
RUN dotnet build "GeoSensePlus.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GeoSensePlus.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GeoSensePlus.Server.dll"]