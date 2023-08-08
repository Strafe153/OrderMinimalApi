# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY ../*.sln ./
COPY ../MinimalApi/*.csproj ./MinimalApi/
COPY ../Application/*.csproj ./Application/
COPY ../Core/*.csproj ./Core/
COPY ../DataAccess/*.csproj ./DataAccess/
COPY ../Tests/Application.Tests/*.csproj ./Tests/Application.Tests/
COPY ../Tests/MinimalApi.Tests/*.csproj ./Tests/MinimalApi.Tests/
RUN dotnet restore

# copy everything else and build app
COPY ../MinimalApi/. ./MinimalApi/
COPY ../Application/. ./Application/
COPY ../Core/. ./Core/
COPY ../DataAccess/. ./DataAccess/
COPY ../Tests/Application.Tests/. ./Tests/Application.Tests/
COPY ../Tests/MinimalApi.Tests/. ./Tests/MinimalApi.Tests/
WORKDIR /app/MinimalApi
RUN dotnet publish -c release -o /app/out

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "MinimalApi.dll"]