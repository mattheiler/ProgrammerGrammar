#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["WebApplication.Core/WebApplication.Core.csproj", "WebApplication.Core/"]
COPY ["WebApplication.Infrastructure/WebApplication.Infrastructure.csproj", "WebApplication.Infrastructure/"]
COPY ["WebApplication.Web/WebApplication.Web.csproj", "WebApplication.Web/"]
RUN dotnet restore "WebApplication.Web/WebApplication.Web.csproj"
COPY . .
WORKDIR "/src/WebApplication.Web"
RUN dotnet build "WebApplication.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApplication.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplication.Web.dll"]