# Build Stage
# Using .NET 9 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0-noble AS build

# Set the working directory inside the container
WORKDIR /app

# Copy solution and project files first for better layer caching
COPY ECommerce.sln ./
COPY ECommerce.API/ECommerce.API.csproj ./ECommerce.API/
COPY ECommerce.Core/ECommerce.Core.csproj ./ECommerce.Core/
COPY ECommerce.Infrastructure/ECommerce.Infrastructure.csproj ./ECommerce.Infrastructure/
COPY ECommerce.Tests/ECommerce.Tests.csproj ./ECommerce.Tests/

# Restore the project dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build and publish the application
RUN dotnet publish ECommerce.API/ECommerce.API.csproj -c Release -o /app/out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble AS runtime

WORKDIR /app
COPY --from=build /app/out .

# Expose port 5090
EXPOSE 5090

# Set the entry point for the container
ENTRYPOINT ["dotnet", "ECommerce.API.dll"]