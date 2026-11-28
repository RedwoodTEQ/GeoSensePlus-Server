using GeoSensePlus.Data.DatabaseModels.Messaging;
using GeoSensePlus.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoSensePlus.Core.Services;

public interface IDataPointService
{
    Task<List<DataPoint>> AddPointsAsync(IEnumerable<string> names, int parentGroupId);
    Task<DataPoint> FindPointByPathAsync(string path);
    Task<string> GetFullPathOfPointAsync(int pointId);
    Task<bool> MovePointsAsync(IEnumerable<int> pointIds, int targetGroupId);
    Task<bool> RemovePointsAsync(IEnumerable<int> pointIds);
}

public class DataPointService : IDataPointService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DataPointService> _logger;
    private readonly IPointGroupService _groupService;

    public DataPointService(ApplicationDbContext context, ILogger<DataPointService> logger, IPointGroupService groupService)
    {
        _context = context;
        _logger = logger;
        _groupService = groupService;
    }

    /// <summary>
    /// Gets the full path of a point (e.g. "Root/Child/PointName")
    /// </summary>
    /// <param name="pointId">ID of point</param>
    /// <returns>Full path string or null if point not found</returns>
    public async Task<string?> GetFullPathOfPointAsync(int pointId)
    {
        var point = await _context.Set<DataPoint>()
            .Include(p => p.Parent)
            .FirstOrDefaultAsync(p => p.Id == pointId);

        if (point == null)
            return null;

        var groupPath = await _groupService.GetFullPathOfGroupAsync(point.ParentId);
        return groupPath == null ? null : $"{groupPath}/{point.Name}";
    }

    /// <summary>
    /// Finds a point by its full path (e.g. "Root/Child/PointName")
    /// </summary>
    /// <param name="path">Full path to point</param>
    /// <returns>Found point or null</returns>
    public async Task<DataPoint> FindPointByPathAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length < 2)
            return null;

        var groupPath = string.Join('/', segments.Take(segments.Length - 1));
        var pointName = segments.Last();

        var group = await _groupService.FindGroupByPathAsync(groupPath);
        if (group == null)
            return null;

        var point = await _context.Set<DataPoint>()
            .Include(p => p.Parent)
            .FirstOrDefaultAsync(p => p.Name == pointName && p.ParentId == group.Id);

        return point;
    }

    /// <summary>
    /// Creates multiple points under a parent group
    /// </summary>
    /// <param name="names">Names of points to create</param>
    /// <param name="parentGroupId">ID of parent group</param>
    /// <returns>List of created points</returns>
    /// <exception cref="ArgumentException">Thrown if parent group doesn't exist</exception>
    public async Task<List<DataPoint>> AddPointsAsync(IEnumerable<string> names, int parentGroupId)
    {
        // Ensure parent group exists
        if (!await _context.Set<PointGroup>().AnyAsync(g => g.Id == parentGroupId))
            throw new ArgumentException("Parent group not found");

        // Ensure names is not empty or all whitespace
        var distinctNames = names.Distinct().ToList();
        if (!distinctNames.Any())
            return new List<DataPoint>();

        // Get all existing points in one query
        var existingNames = await _context.Set<DataPoint>()
            .Where(p => p.ParentId == parentGroupId && distinctNames.Contains(p.Name))
            .Select(p => p.Name)
            .ToListAsync();

        var duplicates = existingNames.ToHashSet();
        var uniqueNames = distinctNames.Except(duplicates).ToList();

        foreach (var duplicate in existingNames)
        {
            _logger.LogInformation("Skipping duplicate point name '{Name}' under group {GroupId}", duplicate, parentGroupId);
        }

        if (!uniqueNames.Any())
            return new List<DataPoint>();

        var points = uniqueNames.Select(n => new DataPoint { Name = n, ParentId = parentGroupId }).ToList();
        _context.Set<DataPoint>().AddRange(points);
        await _context.SaveChangesAsync();
        return points;
    }

    /// <summary>
    /// Removes multiple points by ID
    /// </summary>
    /// <param name="pointIds">IDs of points to remove</param>
    /// <returns>True if any points were found and removed</returns>
    public async Task<bool> RemovePointsAsync(IEnumerable<int> pointIds)
    {
        var points = await _context.Set<DataPoint>()
            .Where(p => pointIds.Contains(p.Id))
            .ToListAsync();

        if (!points.Any()) return false;

        _context.Set<DataPoint>().RemoveRange(points);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Moves multiple points to a new parent group
    /// </summary>
    /// <param name="pointIds">IDs of points to move</param>
    /// <param name="targetGroupId">ID of new parent group</param>
    /// <returns>True if any points were found and moved</returns>
    /// <exception cref="ArgumentException">Thrown if target group doesn't exist</exception>
    public async Task<bool> MovePointsAsync(IEnumerable<int> pointIds, int targetGroupId)
    {
        if (!await _context.Set<PointGroup>().AnyAsync(g => g.Id == targetGroupId))
            throw new ArgumentException("Target group not found");

        var points = await _context.Set<DataPoint>()
            .Where(p => pointIds.Contains(p.Id))
            .ToListAsync();

        if (!points.Any()) return false;

        foreach (var point in points)
        {
            point.ParentId = targetGroupId;
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
