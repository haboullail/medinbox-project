# ----------- STAGE 1: Build the application -------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory inside the container
WORKDIR /src

# Copy csproj files individually to leverage Docker cache during restore
COPY ["src/Medinbox.WebAPI/Medinbox.WebAPI.csproj", "Medinbox.WebAPI/"]
COPY ["src/Medinbox.Application/Medinbox.Application.csproj", "Medinbox.Application/"]
COPY ["src/Medinbox.Domain/Medinbox.Domain.csproj", "Medinbox.Domain/"]
COPY ["src/Medinbox.Infrastructure/Medinbox.Infrastructure.csproj", "Medinbox.Infrastructure/"]

# Restore NuGet packages
RUN dotnet restore "Medinbox.WebAPI/Medinbox.WebAPI.csproj"

# Copy the full source code into the container
COPY . .

# Set working directory to the WebAPI project
WORKDIR "/src/src/Medinbox.WebAPI"

# Publish the application to the /app/publish directory (self-contained)
RUN dotnet publish "Medinbox.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ----------- STAGE 2: Create runtime image -------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Set working directory for runtime
WORKDIR /app

# Expose the port used by the WebAPI
EXPOSE 8082

# Copy published files from the build stage
COPY --from=build /app/publish .

# Start the application
ENTRYPOINT ["dotnet", "Medinbox.WebAPI.dll"]
