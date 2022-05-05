FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

# Set work dir
WORKDIR /app

# Copy all files
COPY src/nhsapp.sample.web.integration/ .

# Publish
RUN dotnet publish *.csproj --configfile nuget.config -c Release -o out

# Run app
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
