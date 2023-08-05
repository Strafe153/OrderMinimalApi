# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY ../*.sln ./
COPY ../MinimalApi/*.csproj ./MinimalApi/
COPY ../Tests/Services.Tests/*.csproj ./Tests/Services.Tests/
COPY ../Tests/Endpoints.Tests/*.csproj ./Tests/Endpoints.Tests/
RUN dotnet restore

# copy everything else and build app
COPY ../MinimalApi/. ./MinimalApi/
COPY ../Tests/Services.Tests/. ./Tests/Services.Tests/
COPY ../Tests/Endpoints.Tests/. ./Tests/Endpoints.Tests/
WORKDIR /app/MinimalApi
RUN dotnet publish -c release -o /app/out

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "MinimalApi.dll"]