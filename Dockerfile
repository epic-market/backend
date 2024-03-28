FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore ./EpicMarket.Business.API/EpicMarket.Business.API.csproj
# Build and publish a release
RUN dotnet publish "./EpicMarket.Business.API/EpicMarket.Business.API.csproj" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /api
COPY --from=build-env /src/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "EpicMarket.Business.API.dll"]