FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy csproj and restore as distinct layers
COPY ../*.sln ./
COPY ../MinimalApi/*.csproj ./MinimalApi/
COPY ../Application/*.csproj ./Application/
COPY ../Domain/*.csproj ./Domain/
COPY ../DataAccess/*.csproj ./DataAccess/
COPY ../Tests/Application.Tests/*.csproj ./Tests/Application.Tests/
COPY ../Tests/MinimalApi.Tests/*.csproj ./Tests/MinimalApi.Tests/
RUN dotnet restore

# Copy everything else and build app
COPY ../MinimalApi/. ./MinimalApi/
COPY ../Application/. ./Application/
COPY ../Domain/. ./Domain/
COPY ../DataAccess/. ./DataAccess/
COPY ../Tests/Application.Tests/. ./Tests/Application.Tests/
COPY ../Tests/MinimalApi.Tests/. ./Tests/MinimalApi.Tests/
WORKDIR /source/MinimalApi
RUN dotnet publish -c release -o /app

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "MinimalApi.dll"]