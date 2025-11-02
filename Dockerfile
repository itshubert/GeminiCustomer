# Use the official .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copy solution file and project files
COPY GeminiCustomer.sln ./
COPY src/GeminiCustomer.Api/GeminiCustomer.Api.csproj ./src/GeminiCustomer.Api/
COPY src/GeminiCustomer.Application/GeminiCustomer.Application.csproj ./src/GeminiCustomer.Application/
COPY src/GeminiCustomer.Contracts/GeminiCustomer.Contracts.csproj ./src/GeminiCustomer.Contracts/
COPY src/GeminiCustomer.Domain/GeminiCustomer.Domain.csproj ./src/GeminiCustomer.Domain/
COPY src/GeminiCustomer.Infrastructure/GeminiCustomer.Infrastructure.csproj ./src/GeminiCustomer.Infrastructure/

# Restore NuGet packages
RUN dotnet restore GeminiCustomer.sln

# Copy the entire source code
COPY . ./

# Build and publish the API project
RUN dotnet publish src/GeminiCustomer.Api/GeminiCustomer.Api.csproj -c Release -o out

# Use the official .NET 9.0 ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build-env /app/out .

# Install essential debugging tools as root
RUN apt-get update && \
    apt-get install -y \
    curl \
    iputils-ping \
    telnet \
    dnsutils \
    net-tools \
    wget \
    && rm -rf /var/lib/apt/lists/* \
    && apt-get clean

# Create a non-root user for security
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Set the entry point for the container
ENTRYPOINT ["dotnet", "GeminiCustomer.Api.dll"]