:: Usage:
:: add-migration Init
:: add-migration Init --output-dir EfGenerated
dotnet ef migrations add %* --context ApplicationDbContext --startup-project ..\GeoSensePlus.WebApi
dotnet ef database update %* --startup-project ..\GeoSensePlus.WebApi