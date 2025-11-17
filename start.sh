#!/bin/bash
echo "Starting application..."
echo "PORT: ${PORT:-8080}"
export ASPNETCORE_URLS="http://0.0.0.0:${PORT:-8080}"
echo "ASPNETCORE_URLS: $ASPNETCORE_URLS"
exec dotnet EstoqueDB.dll
