#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
# EXPOSE 80
# EXPOSE 443
EXPOSE 5001
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GeoSensePlus.WebApi/GeoSensePlus.WebApi.csproj", "GeoSensePlus.WebApi/"]
COPY ["GeoSensePlus.App.ProgressTracking/GeoSensePlus.App.ProgressTracking.csproj", "GeoSensePlus.App.ProgressTracking/"]
COPY ["GeoSensePlus.Core/GeoSensePlus.Core.csproj", "GeoSensePlus.Core/"]
COPY ["GeoSensePlus.App.AssetTracking/GeoSensePlus.App.AssetTracking.csproj", "GeoSensePlus.App.AssetTracking/"]
COPY ["GeoSensePlus.Firestore/GeoSensePlus.Firestore.csproj", "GeoSensePlus.Firestore/"]
COPY ["GeoSensePlus.Data/GeoSensePlus.Data.csproj", "GeoSensePlus.Data/"]
COPY ["GeoSensePlus.Mongo/GeoSensePlus.Mongo.csproj", "GeoSensePlus.Mongo/"]
COPY ["GeoSensePlus.Mqtt/GeoSensePlus.Mqtt.csproj", "GeoSensePlus.Mqtt/"]
RUN dotnet restore "GeoSensePlus.WebApi/GeoSensePlus.WebApi.csproj"
COPY . .
WORKDIR "/src/GeoSensePlus.WebApi"
RUN dotnet build "GeoSensePlus.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GeoSensePlus.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GeoSensePlus.WebApi.dll"]