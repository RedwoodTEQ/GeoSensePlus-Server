dotnet pack -c Release -o ../Publish/

:: %1 should be "install" or "update"
:: dotnet tool %1 -g GeoSensePlus.Server --add-source ./bin/NuPkg

:: dotnet publish -c Release -o ./bin/Publish/GeoSensePlus.Cli
