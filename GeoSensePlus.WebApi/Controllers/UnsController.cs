using Microsoft.AspNetCore.Mvc;

namespace GeoSensePlus.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UnsController : ControllerBase
{
    public UnsController()
    {
    }

    [HttpGet("{*path}")]
    public IActionResult GetPoint(string path)
    {
        return Ok($"Received request for UNS with path: {path}");
    }
}
