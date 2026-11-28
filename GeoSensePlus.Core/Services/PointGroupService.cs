using GeoSensePlus.Data.DatabaseModels.Messaging;
using GeoSensePlus.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoSensePlus.Core.Services;

public interface IPointGroupService
{
    Task<PointGroup> AddGroupAsync(string name, int? parentId = null);
    Task<PointGroup> FindGroupByPathAsync(string path);
    Task<string> GetFullPathOfGroupAsync(int groupId);
    Task<List<PointGroup>> GetNestedTreeAsync(int? rootId = null);
    Task<bool> MoveGroupAsync(int groupId, int? newParentId);
    Task<bool> RemoveGroupAsync(int id, bool deleteWithChildren = false, bool cascade = false);
}

public class PointGroupService : IPointGroupService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PointGroupService> _logger;

    public PointGroupService(ApplicationDbContext context, ILogger<PointGroupService> logger)
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
         * See comment in <see cref="GetAllChildGroupsFlatAsync(int?, bool)"/>
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
}
