# ---- Build Stage ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Kopieren Sie die Projektdateien für das Wiederherstellen von Abhängigkeiten
COPY ["SmartTaskAPI.sln", "./"]
COPY ["src/Api/Api.csproj", "./src/Api/"]
COPY ["src/Application/Application.csproj", "./src/Application/"]
COPY ["src/Domain/Domain.csproj", "./src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "./src/Infrastructure/"]
COPY ["src/Tests/UnitTests/UnitTests.csproj", "./src/Tests/UnitTests/"]
COPY ["src/Tests/IntegrationTests/IntegrationTests.csproj", "./src/Tests/IntegrationTests/"]

# Stell sicher, dass die Abhängigkeiten wiederhergestellt werden
RUN dotnet restore "SmartTaskAPI.sln"

# Kopiere alle Dateien
COPY . .

# Baue das Projekt
WORKDIR /src/src/Api
RUN dotnet publish -c Release -o /app/publish

# ---- Runtime Stage ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
