using DnsClient.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GeoSensePlus.WebApi.Controllers.Messaging;

/// <summary>
/// UNS points controller.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PointsController : ControllerBase
{
    ILogger<PointsController> _logger;
    public PointsController(ILogger<PointsController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{*path}")]
    public IActionResult Get(string path)
    {
        return Ok($"Received request for UNS with path: {path}");
    }

    [HttpPost]
    public IActionResult Post([FromBody] object data)
    {
        _logger.LogInformation("Received POST request for UNS with data: {Data}", data);
        return Ok($"Received POST request for UNS, data: {data.ToString()}");
    }
}
