:: remove and unapply the last migration
dotnet ef migrations remove --context ApplicationDbContext --startup-project ..\GeoSensePlus.WebApi --force