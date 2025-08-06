# Étape 1 : Image de base pour l'exécution
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Étape 2 : Image pour la compilation
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copier les fichiers .csproj de chaque projet
COPY ["src/Medinbox.WebAPI/Medinbox.WebAPI.csproj", "src/Medinbox.WebAPI/"]
COPY ["src/Medinbox.Application/Medinbox.Application.csproj", "src/Medinbox.Application/"]
COPY ["src/Medinbox.Domain/Medinbox.Domain.csproj", "src/Medinbox.Domain/"]
COPY ["src/Medinbox.Infrastructure/Medinbox.Infrastructure.csproj", "src/Medinbox.Infrastructure/"]

# Restauration des packages
RUN dotnet restore "src/Medinbox.WebAPI/Medinbox.WebAPI.csproj"

# Copier tous les fichiers source
COPY . .

# Build de l'application
WORKDIR "/src/src/Medinbox.WebAPI"
RUN dotnet build "Medinbox.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Étape 3 : Publication
FROM build AS publish
RUN dotnet publish "Medinbox.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Étape 4 : Image finale d'exécution
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Medinbox.WebAPI.dll"]
