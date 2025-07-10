using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.Map;

public class GeofenceEntity : NamedEntity<int>
{
    /// <summary>
    /// Empty: indicate it's an outdoor POI or a map marker
    /// Rectangle
    /// Circle
    /// </summary>
    [Column("shape")]
    public string Shape { get; set; }
}
[Table("geofence", Schema = SchemaNames.map)]
public class Geofence : GeofenceEntity
{
}
