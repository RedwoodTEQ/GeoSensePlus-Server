using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.Location;

public class FloorEntity : BaseEntity<int>
{
    [Column("level_number")]
    public int LevelNumber { get; set; }

    [Column("building_id")]
    public int? BuildingId { get; set; } // Foreign key to Building
}

[Table("floor", Schema = SchemaNames.location)]
public class Floor : BaseEntity<int>
{
    public Building Building { get; set; }

    public List<FloorPlan> Floorplans { get; set; }
}
