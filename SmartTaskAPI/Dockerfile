# ---- Build Stage ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution
COPY SmartTaskAPI.sln ./

# Copy each project file explicitly (so restore can be cached)
COPY src/Domain/Domain.csproj src/Domain/
COPY src/Application/Application.csproj src/Application/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/
COPY src/Api/Api.csproj src/Api/

# Restore dependencies
RUN dotnet restore SmartTaskAPI.sln

# Copy the rest of the code
COPY . .

# Build & publish
WORKDIR /src/Api
RUN dotnet publish -c Release -o /app/publish

# ---- Runtime Stage ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Api.dll"]
