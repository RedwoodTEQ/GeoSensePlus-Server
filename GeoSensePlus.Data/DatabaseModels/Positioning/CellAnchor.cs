using GeoSensePlus.Data.DatabaseModels.Location;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.Tracking;

public class CellAnchorEntity : NamedEntity<int>
{
    [Column("area_id")]
    public int? AreaId { get; set; }  // foreign key to Area

    [Column("cell_hub_id")]
    public int? CellHubId { get; set; }    // foreign key to CellHub
}

/// <summary>
/// Renamed from "edge", to keep consistant with other tracking devices' naming.
/// 
/// A cell anchor is a combination of a BLE reader and a lora gateway, it
/// reads BLE beacon signal nearby and transmit the location messages via
/// lora to a hub in the local building.
/// </summary>
[Table("cell_anchor", Schema = SchemaNames.positioning)]
public class CellAnchor : CellAnchorEntity
{
    public Area Area { get; set; }
    public CellHub CellHub { get; set; }
    public List<CellTag> CellTags { get; set; } = new List<CellTag>();
}
