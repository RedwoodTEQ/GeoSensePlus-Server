using GeoSensePlus.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GeoSensePlus.WebApi.DynamicRoute;

public class SearchValueTransformer : DynamicRouteValueTransformer
{
    ILogger<SearchValueTransformer> _logger;
    ApplicationDbContext _ctx;

    public SearchValueTransformer(ILogger<SearchValueTransformer> logger, ApplicationDbContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }

    public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        var productString = values["product"] as string;
        _logger.LogInformation("Product string: {ProductString}", productString);

        var sensor = _ctx.Sensors.FirstOrDefault();
        if(sensor != null)
            _logger.LogInformation($"1st sensor name: {sensor.Name}");

        values["controller"] = "System";
        values["action"] = "Product";
        values["product"] = $"transformer modified product: {productString}";

        return new ValueTask<RouteValueDictionary>(values);
    }
}
