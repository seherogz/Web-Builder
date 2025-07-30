FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HotelWebsiteBuilder.csproj", "./"]
RUN dotnet restore "HotelWebsiteBuilder.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "HotelWebsiteBuilder.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HotelWebsiteBuilder.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:StaticWebAssetsEnabled=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelWebsiteBuilder.dll"] 