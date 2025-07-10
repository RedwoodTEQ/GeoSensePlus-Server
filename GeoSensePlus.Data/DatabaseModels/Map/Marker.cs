using GeoSensePlus.Data.DatabaseModels.Location;

namespace GeoSensePlus.Data.DatabaseModels.Map;

public class MarkerEntity : NamedEntity<int>
{
    public double AxisX { get; set; } = -1;
    public double AxisY { get; set; } = -1;
    public bool IsPoi { get; set; } = false;
    public int? FloorPlanId { get; set; }  // foreign key to FloorPlan
}

/// <summary>
/// Renamed from "Poi"
/// Indoor use only, if need an outdoor POI, use Geofence instead
/// </summary>
public class Marker : MarkerEntity
{
    public FloorPlan Floorplan { get; set; }

}
