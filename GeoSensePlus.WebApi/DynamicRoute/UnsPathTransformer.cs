using GeoSensePlus.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GeoSensePlus.WebApi.DynamicRoute;

public class UnsPathTransformer : DynamicRouteValueTransformer
{
    ILogger<UnsPathTransformer> _logger;
    ApplicationDbContext _ctx;

    public UnsPathTransformer(ILogger<UnsPathTransformer> logger, ApplicationDbContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }

    public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        var pathString = values["path"] as string;
        _logger.LogInformation("Path string: {ProductString}", pathString);

        // _todo: change to use PointService
        var sensor = _ctx.Sensors.FirstOrDefault();
        if(sensor != null)
            _logger.LogInformation($"1st sensor name: {sensor.Name}");

        // _todo: change to PointsController
        values["controller"] = "System";
        values["action"] = "Product";
        values["product"] = $"transformer modified product: {pathString}";

        return new ValueTask<RouteValueDictionary>(values);
    }
}
