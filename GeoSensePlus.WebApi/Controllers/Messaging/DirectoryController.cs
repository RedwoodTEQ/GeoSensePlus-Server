using DnsClient.Internal;
using GeoSensePlus.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GeoSensePlus.WebApi.Controllers.Messaging;

public record AddGroupRequest(string Name, int? ParentId);
public record MoveGroupRequest(int? NewParentId);
public record AddPointsRequest(string[] Names, int GroupId);
public record MovePointsRequest(int[] Ids, int NewGroupId);

/// <summary>
/// State directory service controller, which is actually an UNS implementation.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DirectoryController : ControllerBase
{
    private readonly IDirectoryService _service;
    ILogger<DirectoryController> _logger;
    public DirectoryController(ILogger<DirectoryController> logger, IDirectoryService service)
    {
        _logger = logger;
        _service = service;
    }

    #region test methods
    [HttpGet("test/{*path}")]
    public IActionResult Get(string path)
    {
        return Ok($"Received request for UNS with path: {path}");
    }

    [HttpPost("test")]
    public IActionResult Post([FromBody] object data)
    {
        _logger.LogInformation("Received POST request for UNS with data: {Data}", data);
        return Ok($"Received POST request for UNS, data: {data.ToString()}");
    }
    #endregion

    [HttpPost("group")]
    public async Task<IActionResult> AddGroup([FromBody] AddGroupRequest request)
    {
        var group = await _service.AddGroupAsync(request.Name, request.ParentId);
        return Ok(group);
    }

    [HttpDelete("group/{id}")]
    public async Task<IActionResult> RemoveGroup(int id, [FromQuery] bool deleteWithChildren = false, [FromQuery] bool cascade = false)
    {
        var result = await _service.RemoveGroupAsync(id, deleteWithChildren, cascade);
        return result ? Ok() : NotFound();
    }

    [HttpPut("group/{id}/move")]
    public async Task<IActionResult> MoveGroup(int id, [FromBody] MoveGroupRequest request)
    {
        var success = await _service.MoveGroupAsync(id, request.NewParentId);
        return success ? Ok() : NotFound();
    }

    [HttpGet("group/{id}/path")]
    public async Task<IActionResult> GetGroupPath(int id)
    {
        var path = await _service.GetFullPathAsync(id);
        return path == null ? NotFound() : Ok(path);
    }

    [HttpGet("group/{id}/subtree")]
    public async Task<IActionResult> GetSubtree(int id)
    {
        var nodes = await _service.GetNestedTreeAsync(id);
        return Ok(nodes);
    }

    [HttpPost("points")]
    public async Task<IActionResult> AddPoints([FromBody] AddPointsRequest request)
    {
        var result = await _service.AddPointsAsync(request.Names, request.GroupId);
        return Ok(result);
    }

    [HttpDelete("points")]
    public async Task<IActionResult> RemovePoints([FromBody] int[] ids)
    {
        var result = await _service.RemovePointsAsync(ids);
        return result ? Ok() : NotFound();
    }

    [HttpPut("points/move")]
    public async Task<IActionResult> MovePoints([FromBody] MovePointsRequest request)
    {
        var result = await _service.MovePointsAsync(request.Ids, request.NewGroupId);
        return result ? Ok() : NotFound();
    }

    [HttpGet("point/{id}/path")]
    public async Task<IActionResult> GetPointPath(int id)
    {
        var path = await _service.GetFullPathOfPointAsync(id);
        return path == null ? NotFound() : Ok(path);
    }
}