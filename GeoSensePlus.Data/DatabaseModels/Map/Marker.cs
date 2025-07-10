using GeoSensePlus.Data.DatabaseModels.Location;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.Map;

public class MarkerEntity : NamedEntity<int>
{
    [Column("axis_x")]
    public double AxisX { get; set; } = -1;

    [Column("axis_y")]
    public double AxisY { get; set; } = -1;

    [Column("is_poi")]
    public bool IsPoi { get; set; } = false;

    [Column("floor_plan_id")]
    public int? FloorPlanId { get; set; }  // foreign key to FloorPlan
}

/// <summary>
/// Renamed from "Poi"
/// Indoor use only, if need an outdoor POI, use Geofence instead
/// </summary>
[Table("marker", Schema = SchemaNames.map)]
public class Marker : MarkerEntity
{
    public FloorPlan Floorplan { get; set; }

}
