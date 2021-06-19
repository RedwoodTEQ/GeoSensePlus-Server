@echo off
SET APP_PORT=5200
:: dapr run --app-id entry --dapr-http-port 3500
:: localhost:3500/v1.0/invoke/geosenseplus-server/method/system/version
dapr run --app-id geosenseplus-server --app-port %APP_PORT% -- dotnet watch run --urls http://localhost:%APP_PORT%
@echo on
