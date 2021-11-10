#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG EXCLUDE_CLIENT=false

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM node:10.15-alpine AS client
WORKDIR /app
COPY WebApplication.Web/App ./
RUN [[ ${EXCLUDE_CLIENT} = false ]] || npm install 
RUN [[ ${EXCLUDE_CLIENT} = false ]] || npm run build -- --prod

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
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
COPY --from=publish /app/publish ./
COPY --from=client /app/dist ./App/dist
ENTRYPOINT ["dotnet", "WebApplication.Web.dll"]
