#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

COPY ["ProcessService.Worker/ProcessService.Worker.csproj", "ProcessService.Worker/"]

RUN --mount=type=secret,id=github_token \
    dotnet nuget add source --username abdurrahim373 --password $(cat /run/secrets/github_token) --store-password-in-clear-text --name github "https://nuget.pkg.github.com/letslearn373/index.json"

RUN dotnet restore "ProcessService.Worker/ProcessService.Worker.csproj"
COPY . .
WORKDIR "/src/ProcessService.Worker"
RUN dotnet build "ProcessService.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProcessService.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProcessService.Worker.dll"]