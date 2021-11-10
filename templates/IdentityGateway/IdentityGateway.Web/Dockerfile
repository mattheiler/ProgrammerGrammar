#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["IdentityGateway.Core/IdentityGateway.Core.csproj", "IdentityGateway.Core/"]
COPY ["IdentityGateway.Infrastructure/IdentityGateway.Infrastructure.csproj", "IdentityGateway.Infrastructure/"]
COPY ["IdentityGateway.Web/IdentityGateway.Web.csproj", "IdentityGateway.Web/"]
RUN dotnet restore "IdentityGateway.Web/IdentityGateway.Web.csproj"
COPY . .
WORKDIR "/src/IdentityGateway.Web"
RUN dotnet build "IdentityGateway.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityGateway.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityGateway.Web.dll"]
