using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Location;

public class BuildingEntity : NamedEntity<int>
{
    [Column("site_id")]
    public int? SiteId { get; set; } // Foreign key to Site
}

[Table("building", Schema = SchemaNames.location)]
public class Building : BuildingEntity
{
    [ForeignKey("SiteId")]
    public Site Site { get; set; }
    public List<FloorPlan> FloorPlans { get; set; } = new List<FloorPlan>();

    public static Building Create(ApplicationDbContext ctx, string name, int siteId)
    {
        var building = new Building
        {
            Name = name,
            SiteId = siteId
        };
        ctx.Buildings.Add(building);
        return building;
    }
}
