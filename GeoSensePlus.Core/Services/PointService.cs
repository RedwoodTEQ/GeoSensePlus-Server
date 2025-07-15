using GeoSensePlus.Data;
using GeoSensePlus.Data.DatabaseModels.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoSensePlus.Core.Services;

public class PointService
{
    private readonly ApplicationDbContext _context;

    public PointService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PointGroup> AddGroupAsync(string name, int? parentId = null)
    {
        if (parentId.HasValue && !await _context.Set<PointGroup>().AnyAsync(g => g.Id == parentId))
            throw new ArgumentException("Parent not found");

        var group = new PointGroup { Name = name, ParentId = parentId };
        _context.Set<PointGroup>().Add(group);
        await _context.SaveChangesAsync();
        return group;
    }

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
            var subtree = await GetSubtreeAsync(id);

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

    public async Task<bool> MoveGroupAsync(int groupId, int? newParentId)
    {
        var group = await _context.Set<PointGroup>().FindAsync(groupId);
        if (group == null) return false;

        if (newParentId == group.Id)
            throw new ArgumentException("Cannot move under itself.");

        if (newParentId.HasValue)
        {
            var subtree = await GetSubtreeAsync(groupId);
            if (subtree.Any(g => g.Id == newParentId))
                throw new InvalidOperationException("Cannot move under its descendant.");
        }

        group.ParentId = newParentId;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> GetFullPathAsync(int groupId)
    {
        var sql = @"
            WITH RECURSIVE path_to_root AS (
                SELECT id, name, parent_id FROM groups WHERE id = {0}
                UNION ALL
                SELECT g.id, g.name, g.parent_id
                FROM groups g
                INNER JOIN path_to_root pt ON pt.parent_id = g.id
            )
            SELECT name FROM path_to_root;
        ";

        var names = await _context.Database.SqlQueryRaw<string>(sql, groupId).ToListAsync();
        if (!names.Any()) return null;

        names.Reverse();
        return string.Join('/', names);
    }

    public async Task<List<PointGroup>> GetSubtreeAsync(int rootId)
    {
        var sql = @"
            WITH RECURSIVE group_tree AS (
                SELECT id, name, parent_id, is_deleted FROM groups WHERE id = {0}
                UNION ALL
                SELECT g.id, g.name, g.parent_id, g.is_deleted
                FROM groups g
                INNER JOIN group_tree gt ON g.parent_id = gt.id
            )
            SELECT * FROM group_tree;
        ";

        return await _context.Set<PointGroup>().FromSqlRaw(sql, rootId).ToListAsync();
    }

    public async Task<List<PointGroup>> GetNestedTreeAsync(int rootId)
    {
        var flat = await GetSubtreeAsync(rootId);
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
    public async Task<PointGroup?> FindGroupByPathAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return null;

        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0) return null;

        PointGroup? current = null;

        foreach (var name in segments)
        {
            current = await _context.Set<PointGroup>()
                .Where(g => g.Name == name && g.ParentId == (current == null ? null : current.Id))
                .FirstOrDefaultAsync();

            if (current == null) return null;
        }

        return current;
    }

    /// <summary>
    /// Find a point by full path (e.g. "Root/Child/Point")
    /// </summary>
    public async Task<Point?> FindPointByPathAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return null;

        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length < 2) return null;

        var groupPath = string.Join('/', segments.Take(segments.Length - 1));
        var pointName = segments.Last();

        var group = await FindGroupByPathAsync(groupPath);
        if (group == null) return null;

        var point = await _context.Set<Point>()
            .Include(p => p.Parent)
            .FirstOrDefaultAsync(p => p.Name == pointName && p.ParentId == group.Id);

        return point;
    }

    public async Task<List<Point>> AddPointsAsync(IEnumerable<string> names, int parentGroupId)
    {
        if (!await _context.Set<PointGroup>().AnyAsync(g => g.Id == parentGroupId))
            throw new ArgumentException("Parent group not found");

        var points = names.Select(n => new Point { Name = n, ParentId = parentGroupId }).ToList();
        _context.Set<Point>().AddRange(points);
        await _context.SaveChangesAsync();
        return points;
    }

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

    public async Task<string?> GetFullPathOfPointAsync(int pointId)
    {
        var point = await _context.Set<Point>()
            .Include(p => p.Parent)
            .FirstOrDefaultAsync(p => p.Id == pointId);

        if (point == null) return null;

        var groupPath = await GetFullPathAsync(point.ParentId);
        return groupPath == null ? null : $"{groupPath}/{point.Name}";
    }
}
