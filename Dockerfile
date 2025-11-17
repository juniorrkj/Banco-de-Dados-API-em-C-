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

# Railway fornece PORT automaticamente (configurado no Program.cs)
CMD ["dotnet", "EstoqueDB.dll"]
