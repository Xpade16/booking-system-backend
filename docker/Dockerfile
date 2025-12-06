# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["src/BookingSystem.Api/BookingSystem.Api.csproj", "BookingSystem.Api/"]
COPY ["src/BookingSystem.Application/BookingSystem.Application.csproj", "BookingSystem.Application/"]
COPY ["src/BookingSystem.Domain/BookingSystem.Domain.csproj", "BookingSystem.Domain/"]
COPY ["src/BookingSystem.Infrastructure/BookingSystem.Infrastructure.csproj", "BookingSystem.Infrastructure/"]

RUN dotnet restore "BookingSystem.Api/BookingSystem.Api.csproj"

# Copy everything else and build
COPY src/ .
WORKDIR "/src/BookingSystem.Api"
RUN dotnet build "BookingSystem.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "BookingSystem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookingSystem.Api.dll"]