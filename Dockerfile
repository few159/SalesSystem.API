# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . . 

# Altere para o caminho correto do seu projeto principal
WORKDIR /app/API
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 7089

ENV ASPNETCORE_URLS=http://+:7089

ENTRYPOINT ["dotnet", "SalesSystem.API.dll"]
