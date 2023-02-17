@echo off


SET DEBUG_PATH=%CD%\bin\Debug\net7.0

dotnet %DEBUG_PATH%\GeoSensePlus.Cli.dll %*

:: or set the debug path to system path, then use the following statement to
:: replace the above one:
:: dotnet %~dp0GeoSensePlus.Cli.dll %*
::
:: if this is a asp.net core project and need to run in watch mode, do: 
:: dotnet watch run <args>
@echo on