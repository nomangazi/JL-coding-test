# Build Stage

# Using .NET 9 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0-nobel AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project files to the container
COPY . .
# Restore the project dependencies
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-nobel AS runtime

WORKDIR /app
COPY --from=build /app/out .

# Expose port 5090
EXPOSE 5090
# Set the entry point for the container
ENTRYPOINT ["dotnet", "ECommerce.API.dll"]