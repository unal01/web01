# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["CoreBuilder/CoreBuilder.csproj", "CoreBuilder/"]
COPY ["CoreBuilder.Admin/CoreBuilder.Admin.csproj", "CoreBuilder.Admin/"]

# Restore dependencies
RUN dotnet restore "CoreBuilder.Admin/CoreBuilder.Admin.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/CoreBuilder.Admin"
RUN dotnet build "CoreBuilder.Admin.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "CoreBuilder.Admin.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install SQL Server tools (optional, for debugging)
USER root
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create non-root user for security
RUN useradd -m -u 1000 appuser && chown -R appuser /app
USER appuser

EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "CoreBuilder.Admin.dll"]
