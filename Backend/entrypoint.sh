#!/bin/bash

set -e

echo "Waiting for SQL Server to be ready..."
until /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "YourStrong!Password" -Q "SELECT 1" > /dev/null 2>&1; do
  sleep 1
done

echo "SQL Server is up. Running migrations..."
dotnet ef database update

echo "Starting the application..."
exec "$@"
