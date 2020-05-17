@echo off
SET DEBUG_PATH=%CD%\bin\Debug\netcoreapp3.0
SET ASPNETCORE_ENVIRONMENT=Development

dotnet watch run %DEBUG_PATH%\GeoSensePlus.Server.dll %*

:: or set the debug path to system path, then use the following statement to
:: replace the above one:
:: dotnet %~dp0GeoSensePlus.Server.dll %*
::
:: if this is a asp.net core project and need to run in watch mode, do: 
:: dotnet watch run <args>
@echo on