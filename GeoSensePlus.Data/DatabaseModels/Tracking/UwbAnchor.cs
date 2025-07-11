using GeoSensePlus.Data.DatabaseModels.Location;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.Tracking;

public class UwbAnchorEntity : NamedEntity<int>
{
    [Column("axis_x")]
    public double AxisX { get; set; } = -1;

    [Column("axis_y")]
    public double AxisY { get; set; } = -1;

    [Column("axis_z")]
    public double AxisZ { get; set; } = -1;

    [Column("config", TypeName = "jsonb")]
    public string Config { get; set; }

    [Column("status")]
    public string Status { get; set; }

    /** TODO: confirm with Kai, more properties?
     * Group?
     * Owner?
     * Attribute?
     * Info? -- RL: redundant with "Description"?
     * Extra?
     */

    [Column("floor_plan_id")]
    public int? FloorPlanId { get; set; }  // foreign key to FloorPlan

    [Column("site_id")]
    public int? SiteId { get; set; }       // foreign key to Site
}

[Table("uwb_anchor", Schema = SchemaNames.tracking)]
public class UwbAnchor : UwbAnchorEntity
{
    public FloorPlan Floorplan { get; set; }
    public Site Site { get; set; }

    public static UwbAnchor Create(ApplicationDbContext ctx, string name)
    {
        var anchor = new UwbAnchor { Name = name };
        ctx.UwbAnchors.Add(anchor);
        ctx.SaveChanges();
        return anchor;
    }

    public static UwbAnchor Create(ApplicationDbContext ctx, string name, string config)
    {
        var anchor = new UwbAnchor { Name = name, Config = config };
        ctx.UwbAnchors.Add(anchor);
        ctx.SaveChanges();
        return anchor;
    }
}
