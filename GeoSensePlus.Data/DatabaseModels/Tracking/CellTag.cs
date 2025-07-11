using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.Tracking;

public class CellTagEntity : NamedEntity<int>
{
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("target_id")]
    public int? TargetId { get; set; }   // foreign key to Target
}

/// <summary>
/// A cell tag is just a BLE beacon, which sends out BLE signals
/// </summary>
[Table("cell_tag", Schema = SchemaNames.tracking)]
public class CellTag : CellTagEntity
{
    public Target Target { get; set; }
}
