# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

## Fix de um problema de SSL https://github.com/dotnet/runtime/issues/98797#issuecomment-1961125933
USER root
RUN echo "\n[openssl_init]\nssl_conf = ssl_sect\n\n[ssl_sect]\nsystem_default = system_default_sect\n\n[system_default_sect]\nCipherString = DEFAULT@SECLEVEL=2" >> /etc/ssl/openssl.cnf
USER app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Balcao.API/Balcao.API.csproj", "Balcao.API/"]
COPY ["Balcao.Domain/Balcao.Domain.csproj", "Balcao.Domain/"]
COPY ["Balcao.Infrastructure/Balcao.Infrastructure.csproj", "Balcao.Infrastructure/"]
RUN dotnet restore "./Balcao.API/Balcao.API.csproj"
COPY . .
RUN dotnet dev-certs https --trust
WORKDIR "/src/Balcao.API"
RUN dotnet build "./Balcao.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Balcao.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Balcao.API.dll"]