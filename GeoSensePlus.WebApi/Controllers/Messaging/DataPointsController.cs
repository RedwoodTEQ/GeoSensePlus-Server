using GeoSensePlus.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GeoSensePlus.WebApi.Controllers.Messaging;

/// <summary>
/// Manages data points of the state directory service.
/// </summary>
[ApiController]
[Route("api/data-points")]
public class DataPointsController : ControllerBase
{
    private readonly IDataPointService _service;
    ILogger<DataPointsController> _logger;

    public DataPointsController(ILogger<DataPointsController> logger, IDataPointService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddPoints([FromBody] AddPointsRequest request)
    {
        var result = await _service.AddPointsAsync(request.Names, request.GroupId);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> RemovePoints([FromBody] int[] ids)
    {
        var result = await _service.RemovePointsAsync(ids);
        return result ? Ok() : NotFound();
    }

    [HttpPut("move")]
    public async Task<IActionResult> MovePoints([FromBody] MovePointsRequest request)
    {
        var result = await _service.MovePointsAsync(request.Ids, request.NewGroupId);
        return result ? Ok() : NotFound();
    }

    [HttpGet("{id}/path")]
    public async Task<IActionResult> GetPointPath(int id)
    {
        var path = await _service.GetFullPathOfPointAsync(id);
        return path == null ? NotFound() : Ok(path);
    }
}
