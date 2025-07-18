using GeoSensePlus.Data.DatabaseModels.Messaging;
using GeoSensePlus.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoSensePlus.Core.Services;

public interface IDirectoryService
{
    Task<PointGroup> AddGroupAsync(string name, int? parentId = null);
    Task<List<Point>> AddPointsAsync(IEnumerable<string> names, int parentGroupId);
    Task<PointGroup> FindGroupByPathAsync(string path);
    Task<Point> FindPointByPathAsync(string path);
    Task<string> GetFullPathOfGroupAsync(int groupId);
    Task<string> GetFullPathOfPointAsync(int pointId);
    Task<List<PointGroup>> GetNestedTreeAsync(int rootId);
    Task<bool> MoveGroupAsync(int groupId, int? newParentId);
    Task<bool> MovePointsAsync(IEnumerable<int> pointIds, int targetGroupId);
    Task<bool> RemoveGroupAsync(int id, bool deleteWithChildren = false, bool cascade = false);
    Task<bool> RemovePointsAsync(IEnumerable<int> pointIds);
    Task<bool> AttachValueToPointAsync(int pointId, int valueId);
    Task<List<Point>> GetPointsByValueIdAsync(int valueId);
}

public class DirectoryService : IDirectoryService
{
    private readonly ApplicationDbContext _context;

    public DirectoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new point group with optional parent group
    /// </summary>
    /// <param name="name">Name of the new group</param>
    /// <param name="parentId">Optional parent group ID</param>
    /// <returns>The created PointGroup</returns>
    /// <exception cref="ArgumentException">Thrown if parent group doesn't exist</exception>
    public async Task<PointGroup> AddGroupAsync(string name, int? parentId = null)
    {
        if (parentId.HasValue && !await _context.Set<PointGroup>().AnyAsync(g => g.Id == parentId))
            throw new ArgumentException("Parent not found");

        var group = new PointGroup { Name = name, ParentId = parentId };
        _context.Set<PointGroup>().Add(group);
        await _context.SaveChangesAsync();
        return group;
    }

    /// <summary>
    /// Removes a point group and optionally its children
    /// </summary>
    /// <param name="id">ID of group to remove</param>
    /// <param name="deleteWithChildren">If true, removes child groups recursively</param>
    /// <param name="cascade">If true, physically deletes records. If false, marks as deleted.</param>
    /// <returns>True if group was found and removed</returns>
    /// <exception cref="InvalidOperationException">Thrown if group has children but deleteWithChildren=false</exception>
    public async Task<bool> RemoveGroupAsync(int id, bool deleteWithChildren = false, bool cascade = false)
    {
        var group = await _context.Set<PointGroup>()
            .Include(g => g.Children)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group == null)
            return false;

        if (group.Children.Any() && !deleteWithChildren)
            throw new InvalidOperationException("Group has children. Set deleteWithChildren = true to remove.");

        if (deleteWithChildren)
        {
            var subtree = await GetAllChildGroupsFlatAsync(id);

            if (cascade)
            {
                _context.Set<PointGroup>().RemoveRange(subtree);
            }
            else
            {
                foreach (var g in subtree)
                    g.IsDeleted = true;
            }
        }
        else
        {
            if (cascade)
                _context.Set<PointGroup>().Remove(group);
            else
                group.IsDeleted = true;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Moves a group to be under a new parent group
    /// </summary>
    /// <param name="groupId">ID of group to move</param>
    /// <param name="newParentId">ID of new parent group (null for root)</param>
    /// <returns>True if group was found and moved</returns>
    /// <exception cref="ArgumentException">Thrown if trying to move under itself</exception>
    /// <exception cref="InvalidOperationException">Thrown if trying to move under descendant</exception>
    public async Task<bool> MoveGroupAsync(int groupId, int? newParentId)
    {
        var group = await _context.Set<PointGroup>().FindAsync(groupId);
        if (group == null) return false;

        if (newParentId == group.Id)
            throw new ArgumentException("Cannot move under itself.");

        if (newParentId.HasValue)
        {
            var subtree = await GetAllChildGroupsFlatAsync(groupId);
            if (subtree.Any(g => g.Id == newParentId))
                throw new InvalidOperationException("Cannot move under its descendant.");
        }

        group.ParentId = newParentId;
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Gets the full path of a group as a string (e.g. "Root/Child/Grandchild")
    /// </summary>
    /// <param name="groupId">ID of group to get path for</param>
    /// <returns>Full path string or null if group not found</returns>
    public async Task<string> GetFullPathOfGroupAsync(int groupId)
    {
        var sql = @"
            WITH RECURSIVE path_to_root AS (
                SELECT id, name, parent_id FROM messaging.point_group WHERE id = {0}
                UNION ALL
                SELECT g.id, g.name, g.parent_id
                FROM messaging.point_group g
                INNER JOIN path_to_root pt ON pt.parent_id = g.id
            )
            SELECT name FROM path_to_root
        ";

        var names = await _context.Database.SqlQueryRaw<string>(sql, groupId).ToListAsync();
        if (!names.Any()) return null;

        names.Reverse();
        return string.Join('/', names);
    }
    public async Task<string?> GetFullPathOfPointAsync(int pointId)
    {
        var point = await _context.Set<Point>()
            .Include(p => p.Parent)
            .FirstOrDefaultAsync(p => p.Id == pointId);

        if (point == null)
            return null;

        var groupPath = await GetFullPathOfGroupAsync(point.ParentId);
        return groupPath == null ? null : $"{groupPath}/{point.Name}";
    }

    /// <summary>
    /// Gets all groups in a subtree (flat list) starting from rootId
    /// </summary>
    /// <param name="rootId">ID of root group</param>
    /// <returns>List of all groups in subtree including root</returns>
    private async Task<List<PointGroup>> GetAllChildGroupsFlatAsync(int rootId, bool includePointValues = false)
    {
        // NOTE: don't add ';' at the end of SQL queries in FromSqlRaw, otherwise when it's used with Include(),
        // it will throw an exception
        var sql = @"
            WITH RECURSIVE group_tree AS (
                SELECT id, name, parent_id, is_deleted, description FROM messaging.point_group WHERE id = {0}
                UNION ALL
                SELECT g.id, g.name, g.parent_id, g.is_deleted, g.description
                FROM messaging.point_group g
                INNER JOIN group_tree gt ON g.parent_id = gt.id
            )
            SELECT g.* FROM group_tree g
        ";

        var groupQuery = _context.Set<PointGroup>().FromSqlRaw(sql, rootId);
        List<PointGroup> groups;
        if (includePointValues)
        {
            groups = await groupQuery
                .Include(g => g.Points)
                    .ThenInclude(p => p.Value)
                .ToListAsync();
        }
        else
        {
            groups = await groupQuery.ToListAsync();
        }
        return groups;
    }

    /// <summary>
    /// Gets a nested tree structure starting from rootId
    /// </summary>
    /// <param name="rootId">ID of root group</param>
    /// <returns>List of root groups with Children populated</returns>
    public async Task<List<PointGroup>> GetNestedTreeAsync(int rootId)
    {
        var flat = await GetAllChildGroupsFlatAsync(rootId, true);
        var dict = flat.ToDictionary(g => g.Id);

        foreach (var group in flat)
        {
            group.Children = new List<PointGroup>();
        }

        foreach (var group in flat)
        {
            if (group.ParentId.HasValue && dict.TryGetValue(group.ParentId.Value, out var parent))
            {
                parent.Children.Add(group);
            }
        }

        return flat.Where(g => g.Id == rootId || g.ParentId == null).ToList();
    }

    /// <summary>
    /// Find a group by full path (e.g. "Root/Child")
    /// </summary>
    /// <summary>
    /// Finds a group by its full path (e.g. "Root/Child/Grandchild")
    /// </summary>
    /// <param name="path">Full path to group</param>
    /// <returns>Found group or null</returns>
    public async Task<PointGroup> FindGroupByPathAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0)
            return null;

        PointGroup current = null;

        foreach (var name in segments)
        {
            current = await _context.Set<PointGroup>()
                .Where(g => g.Name == name && g.ParentId == (current == null ? null : current.Id))
                .FirstOrDefaultAsync();

            if (current == null)
                return null;
        }

        return current;
    }

    /// <summary>
    /// Find a point by full path (e.g. "Root/Child/Point")
    /// </summary>
    /// <summary>
    /// Finds a point by its full path (e.g. "Root/Child/PointName")
    /// </summary>
    /// <param name="path">Full path to point</param>
    /// <returns>Found point or null</returns>
    public async Task<Point> FindPointByPathAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length < 2)
            return null;

        var groupPath = string.Join('/', segments.Take(segments.Length - 1));
        var pointName = segments.Last();

        var group = await FindGroupByPathAsync(groupPath);
        if (group == null)
            return null;

        var point = await _context.Set<Point>()
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
    public async Task<List<Point>> AddPointsAsync(IEnumerable<string> names, int parentGroupId)
    {
        if (!await _context.Set<PointGroup>().AnyAsync(g => g.Id == parentGroupId))
            throw new ArgumentException("Parent group not found");

        var points = names.Select(n => new Point { Name = n, ParentId = parentGroupId }).ToList();
        _context.Set<Point>().AddRange(points);
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
        var points = await _context.Set<Point>()
            .Where(p => pointIds.Contains(p.Id))
            .ToListAsync();

        if (!points.Any()) return false;

        _context.Set<Point>().RemoveRange(points);
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

        var points = await _context.Set<Point>()
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

    /// <summary>
    /// Gets the full path of a point (e.g. "Root/Child/PointName")
    /// </summary>
    /// <param name="pointId">ID of point</param>
    /// <returns>Full path string or null if point not found</returns>
    /// <summary>
    /// Attaches a value to a point
    /// </summary>
    /// <param name="pointId">ID of the point to attach to</param>
    /// <param name="valueId">ID of the value to attach</param>
    /// <returns>True if both point and value exist and were successfully attached</returns>
    public async Task<bool> AttachValueToPointAsync(int pointId, int valueId)
    {
        var point = await _context.Set<Point>()
            .FirstOrDefaultAsync(p => p.Id == pointId);
        
        var value = await _context.Set<Value>()
            .FirstOrDefaultAsync(v => v.Id == valueId);

        if (point == null || value == null)
            return false;

        point.ValueId = valueId;
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Gets all points that reference a specific value
    /// </summary>
    /// <param name="valueId">ID of the value to query</param>
    /// <returns>List of points that reference this value</returns>
    public async Task<List<Point>> GetPointsByValueIdAsync(int valueId)
    {
        return await _context.Set<Point>()
            .Where(p => p.ValueId == valueId)
            .Include(p => p.Parent)
            .ToListAsync();
    }
}
