using GeoSensePlus.Data;
using GeoSensePlus.Data.DatabaseModels.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
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
}
