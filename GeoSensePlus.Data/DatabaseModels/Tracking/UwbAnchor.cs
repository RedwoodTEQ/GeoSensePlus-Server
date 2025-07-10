using GeoSensePlus.Data.DatabaseModels.Location;

namespace GeoSensePlus.Data.DatabaseModels.Tracking;

public class UwbAnchorEntity : NamedEntity<int>
{
    public double AxisX { get; set; } = -1;
    public double AxisY { get; set; } = -1;
    public double AxisZ { get; set; } = -1;
    //[Column(TypeName = "jsonb")]
    public string Configuration { get; set; }
    public string Status { get; set; }

    /** TODO: confirm with Kai, more properties?
     * Group?
     * Owner?
     * Attribute?
     * Info? -- RL: redundant with "Description"?
     * Extra?
     */

    public int? FloorPlanId { get; set; }  // foreign key to FloorPlan
    public int? SiteId { get; set; }       // foreign key to Site
}

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
        var anchor = new UwbAnchor { Name = name, Configuration = config };
        ctx.UwbAnchors.Add(anchor);
        ctx.SaveChanges();
        return anchor;
    }
}
