#!/bin/bash

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &

# Wait for SQL Server to start up
echo "Waiting for SQL Server to start..."
sleep 30

# Restore the database
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Your_password123 -Q "
RESTORE DATABASE [ICLOTHING] FROM DISK = N'/var/opt/mssql/backup/ICLOTHING.bak'
WITH MOVE 'ICLOTHING' TO '/var/opt/mssql/data/ICLOTHING.mdf',
     MOVE 'ICLOTHING_log' TO '/var/opt/mssql/data/ICLOTHING_log.ldf',
     REPLACE;"


# Keep the container running
tail -f /dev/null
