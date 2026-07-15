FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy csproj and restore as distinct layers
COPY ../*.sln ./
COPY ../Api/*.csproj ./Api/
COPY ../Api.Tests/*.csproj ./Api.Tests/
RUN dotnet restore

# Copy everything else and build app
COPY ../Api/. ./Api/
COPY ../Api.Tests/. ./Api.Tests/
WORKDIR /source/Api
RUN dotnet publish -c release -o /app

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Api.dll"]