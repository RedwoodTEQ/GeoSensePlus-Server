using GeoSensePlus.Data.DatabaseModels.Tracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Location;

public class AreaEntity : NamedEntity<int>
{
    [Column("floor_plan_id")]
    public int? FloorPlanId { get; set; }   // foreign key to FloorPlan
}

/// <summary>
/// An area is an indoor area, which can be a room, a corridor etc.
/// </summary>
[Table("area", Schema = SchemaNames.location)]
public class Area : AreaEntity
{
    public FloorPlan Floorplan { get; set; }
    public CellAnchor CellAnchor { get; set; }      // CellAnchor is the the principal entity in this ne-to-one relationship, so a int CellAnchorId is not needed here.

    //public List<Target> CacheTargets { get; set; } = new List<Target>();

    public static void Create(ApplicationDbContext ctx, string name)
    {
        ctx.Areas.Add(new Area { Name = name });
        ctx.SaveChanges();
    }
    public static void Create(ApplicationDbContext ctx, string name, string description)
    {
        ctx.Areas.Add(new Area { Name = name, Description = description });
        ctx.SaveChanges();
    }
}
