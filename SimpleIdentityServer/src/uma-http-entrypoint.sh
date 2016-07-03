#!/bin/sh

./wait-for-it.sh $DB_ALIAS:$DB_PORT
bash -c "cd SimpleIdentityServer.Uma.EF && dotnet ef database update"
dotnet run --project SimpleIdentityServer.Uma.Host/project.json --server.urls=http://*:5000