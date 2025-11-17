# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY BD/EstoqueDB.csproj BD/
RUN dotnet restore BD/EstoqueDB.csproj

# Copy everything else and build
COPY BD/ BD/
WORKDIR /app/BD
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/BD/out .

# Expose port
EXPOSE 8080

# Set environment to use port from Railway
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "EstoqueDB.dll"]
