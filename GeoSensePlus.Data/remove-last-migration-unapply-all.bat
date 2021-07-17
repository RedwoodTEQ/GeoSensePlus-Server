:: unapply all migrations from database
dotnet ef database update 0 --startup-project ..\GeoSensePlus.WebApi

:: remove the last migration code
dotnet ef migrations remove --context ApplicationDbContext --startup-project ..\GeoSensePlus.WebApi