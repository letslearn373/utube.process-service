#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

COPY ["ProcessService.Api/ProcessService.Api.csproj", "ProcessService.Api/"]
COPY ["ProcessService.Application/ProcessService.Application.csproj", "ProcessService.Application/"]
COPY ["ProcessService.Infrastructure/ProcessService.Infrastructure.csproj", "ProcessService.Infrastructure/"]

RUN --mount=type=secret,id=github_token \
    dotnet nuget add source --username abdurrahim373 --password $(cat /run/secrets/github_token) --store-password-in-clear-text --name github "https://nuget.pkg.github.com/letslearn373/index.json"

RUN dotnet restore "ProcessService.Api/ProcessService.Api.csproj"
COPY . .
WORKDIR "/src/ProcessService.Api"
RUN dotnet build "ProcessService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProcessService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProcessService.Api.dll"]