#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["WebApplication.Tests/WebApplication.Tests.csproj", "WebApplication.Tests/"]
COPY ["WebApplication.Infrastructure/WebApplication.Infrastructure.csproj", "WebApplication.Infrastructure/"]
COPY ["WebApplication.Core/WebApplication.Core.csproj", "WebApplication.Core/"]
COPY ["WebApplication.Web/WebApplication.Web.csproj", "WebApplication.Web/"]
RUN dotnet restore "WebApplication.Tests/WebApplication.Tests.csproj"
COPY . .
WORKDIR "/src/WebApplication.Tests"
RUN dotnet build "WebApplication.Tests.csproj" -c Release -o /app/build

FROM build AS testrunner
WORKDIR /src/WebApplication.Tests
CMD ["dotnet", "test", "--logger:trx"]