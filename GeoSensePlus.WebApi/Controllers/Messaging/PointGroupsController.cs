using GeoSensePlus.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GeoSensePlus.WebApi.Controllers.Messaging;

/// <summary>
/// Manages point groups of the state directory service.
/// </summary>
[ApiController]
[Route("api/point-groups")]
public class PointGroupsController : ControllerBase
{
    private readonly IDirectoryService _service;
    ILogger<PointGroupsController> _logger;

    public PointGroupsController(ILogger<PointGroupsController> logger, IDirectoryService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddGroup([FromBody] AddGroupRequest request)
    {
        var group = await _service.AddGroupAsync(request.Name, request.ParentId);
        return Ok(group);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveGroup(int id, [FromQuery] bool deleteWithChildren = false, [FromQuery] bool cascade = false)
    {
        var result = await _service.RemoveGroupAsync(id, deleteWithChildren, cascade);
        return result ? Ok() : NotFound();
    }

    [HttpPut("{id}/move")]
    public async Task<IActionResult> MoveGroup(int id, [FromBody] MoveGroupRequest request)
    {
        var success = await _service.MoveGroupAsync(id, request.NewParentId);
        return success ? Ok() : NotFound();
    }

    [HttpGet("{id}/path")]
    public async Task<IActionResult> GetGroupPath(int id)
    {
        var path = await _service.GetFullPathOfGroupAsync(id);
        return path == null ? NotFound() : Ok(path);
    }

    [HttpGet("{id}/subtree")]
    public async Task<IActionResult> GetSubtree(int id)
    {
        var nodes = await _service.GetNestedTreeAsync(id);
        return Ok(nodes);
    }
}
