using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.Tracking;

public class CellHubEntity : NamedEntity<int>
{
    // This class is used for API requests, so it does not need any additional properties.
}

/// <summary>
/// Renamed from gateway (to distinguish from concept of lora gateway).
/// Used for collecting local cell id tracking messages via lora and communicating with cloud services.
/// There is usually only one hub for a building.
/// </summary>
[Table("cell_hub", Schema = SchemaNames.tracking)]
public class CellHub : CellHubEntity
{
    public List<CellAnchor> CellAnchors { get; set; } = new List<CellAnchor>();
    public List<CellTag> CellTags { get; set; } = new List<CellTag>();
}
