#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["IdentityGateway.Tests/IdentityGateway.Tests.csproj", "IdentityGateway.Tests/"]
COPY ["IdentityGateway.Core/IdentityGateway.Core.csproj", "IdentityGateway.Core/"]
COPY ["IdentityGateway.Web/IdentityGateway.Web.csproj", "IdentityGateway.Web/"]
COPY ["IdentityGateway.Infrastructure/IdentityGateway.Infrastructure.csproj", "IdentityGateway.Infrastructure/"]
RUN dotnet restore "IdentityGateway.Tests/IdentityGateway.Tests.csproj"
COPY . .
WORKDIR "/src/IdentityGateway.Tests"
RUN dotnet build "IdentityGateway.Tests.csproj" -c Release -o /app/build

FROM build AS testrunner
WORKDIR /src/IdentityGateway.Tests
CMD ["dotnet", "test", "--logger:trx"]