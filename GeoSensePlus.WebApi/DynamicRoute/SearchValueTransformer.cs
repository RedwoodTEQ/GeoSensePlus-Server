using GeoSensePlus.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GeoSensePlus.WebApi.DynamicRoute;

public class SearchValueTransformer : DynamicRouteValueTransformer
{
    ILogger<SearchValueTransformer> _logger;

    public SearchValueTransformer(ILogger<SearchValueTransformer> logger)
    {
        _logger = logger;
    }

    public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        var productString = values["product"] as string;
        _logger.LogInformation("Product string: {ProductString}", productString);

        //var id = await this._productLocator.FindProduct(“product”, out var controller);

        values["controller"] = "system";
        values["action"] = "Get";
        values["id"] = "version";

        return new ValueTask<RouteValueDictionary>(values);
    }
}
