namespace GeoSensePlus.Data.DatabaseModels.Map;

public class GeofenceEntity : NamedEntity<int>
{
    /// <summary>
    /// Empty: indicate it's an outdoor POI or a map marker
    /// Rectangle
    /// Circle
    /// </summary>
    public string Shape { get; set; }
}
public class Geofence : GeofenceEntity
{
}
