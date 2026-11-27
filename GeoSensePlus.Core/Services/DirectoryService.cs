using GeoSensePlus.Data.DatabaseModels.Messaging;
using GeoSensePlus.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoSensePlus.Core.Services;

public interface IDirectoryService
{
    Task<PointGroup> AddGroupAsync(string name, int? parentId = null);
    Task<List<DataPoint>> AddPointsAsync(IEnumerable<string> names, int parentGroupId);
    Task<PointGroup> FindGroupByPathAsync(string path);
    Task<DataPoint> FindPointByPathAsync(string path);
    Task<string> GetFullPathOfGroupAsync(int groupId);
    Task<string> GetFullPathOfPointAsync(int pointId);
    Task<List<PointGroup>> GetNestedTreeAsync(int? rootId = null);
    Task<bool> MoveGroupAsync(int groupId, int? newParentId);
    Task<bool> MovePointsAsync(IEnumerable<int> pointIds, int targetGroupId);
    Task<bool> RemoveGroupAsync(int id, bool deleteWithChildren = false, bool cascade = false);
    Task<bool> RemovePointsAsync(IEnumerable<int> pointIds);
}

public class DirectoryService : IDirectoryService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DirectoryService> _logger;

    public DirectoryService(ApplicationDbContext context, ILogger<DirectoryService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new point group with optional parent group
    /// </summary>
    /// <param name="name">Name of the new group</param>
    /// <param name="parentId">Optional parent group ID</param>
    /// <returns>The created PointGroup</returns>
    /// <exception cref="ArgumentException">Thrown if parent group doesn't exist</exception>
    private async Task<bool> GroupNameExistsAsync(string name, int? parentId)
    {
        return await _context.Set<PointGroup>()
            .AnyAsync(g => g.Name == name && g.ParentId == parentId && !g.IsDeleted);
    }


    public async Task<PointGroup> AddGroupAsync(string name, int? parentId = null)
    {
        if (parentId.HasValue && !await _context.Set<PointGroup>().AnyAsync(g => g.Id == parentId))
            throw new ArgumentException("Parent not found");

        if (await GroupNameExistsAsync(name, parentId))
            throw new InvalidOperationException($"A group with name '{name}' already exists under this parent");

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
        /**
         * See comment in <see cref="GetAllChildGroupsFlatAsync(int, bool)"/>
         */
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
        var point = await _context.Set<DataPoint>()
            .Include(p => p.Parent)
            .FirstOrDefaultAsync(p => p.Id == pointId);

        if (point == null)
            return null;

        var groupPath = await GetFullPathOfGroupAsync(point.ParentId);
        return groupPath == null ? null : $"{groupPath}/{point.Name}";
    }

    /// <summary>
    /// Gets all groups in a subtree (flat list) starting from rootId, or all groups when rootId is null
    /// </summary>
    /// <param name="rootId">ID of root group, or null to load all groups</param>
    /// <returns>List of all groups in subtree including root</returns>
    private async Task<List<PointGroup>> GetAllChildGroupsFlatAsync(int? rootId, bool includePointValues = false)
    {
        /**
         * NOTE 1: Don't add ';' at the end of SQL queries in FromSqlRaw, otherwise when used with Include(),
         * it will throw an exception.
         * 
         * NOTE 2: Reason of using "SELECT *" in the SQL statement:
         * When EF Core composes over Set<PointGroup>().FromSqlRaw it projects every column of entity PointGroup,
         * and any missing column surfaces as a Postgres 42703 "column does not exist" error at query time.
         * 
         * That is different from Database.SqlQueryRaw<string> in method <see cref="GetFullPathOfGroupAsync(int)"/>,
         * which is a raw SQL query that returns a single column of type string, and EF Core does not project any
         * columns of an entity.
         * 
         * _todo: CTE may need to have a maximum recursion depth limit to avoid infinite loops in case of circular references. Postgres has a default limit of 100, but it can be changed with "SET max_recursion_depth = N;".
         */
        var sql = @"
            WITH RECURSIVE group_tree AS (
                SELECT * FROM messaging.point_group WHERE id = {0}
                UNION ALL
                SELECT g.*
                FROM messaging.point_group g
                INNER JOIN group_tree gt ON g.parent_id = gt.id
            )
            SELECT * FROM group_tree
        ";

        /**
         * NOTE 3: When rootId is null (load all groups), the recursive CTE is unnecessary - a plain
         * Set<PointGroup>() query is used instead, since the tree is assembled in memory anyway.
         */
        var groupQuery = rootId.HasValue
            ? _context.Set<PointGroup>().FromSqlRaw(sql, rootId.Value)
            : _context.Set<PointGroup>();
        List<PointGroup> groups;
        if (includePointValues)
        {
            groups = await groupQuery
                .Include(g => g.DataPoints)
                    .ThenInclude(p => p.DataChannels)
                .ToListAsync();
        }
        else
        {
            groups = await groupQuery.ToListAsync();
        }
        return groups;
    }

    /// <summary>
    /// Gets a nested tree structure starting from rootId, or all trees (forest) when rootId is null
    /// </summary>
    /// <param name="rootId">ID of root group, or null to return all root groups</param>
    /// <returns>List of root groups with Children populated</returns>
    public async Task<List<PointGroup>> GetNestedTreeAsync(int? rootId = null)
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
    public async Task<DataPoint> FindPointByPathAsync(string path)
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

    /// <summary>
    /// Gets the full path of a point (e.g. "Root/Child/PointName")
    /// </summary>
    /// <param name="pointId">ID of point</param>
    /// <returns>Full path string or null if point not found</returns>
    /// <summary>
    /// Gets the full path of a point (e.g. "Root/Child/PointName")
    /// </summary>
}
