using System.Collections.Generic;

namespace GeoSensePlus.Data.DatabaseModels.Location;

public class FloorEntity : NamedEntity<int>
{
    public int LevelNumber { get; set; }
    public int? BuildingId { get; set; } // Foreign key to Building
}

public class Floor : NamedEntity<int>
{
    public Building Building { get; set; }

    public List<FloorPlan> Floorplans { get; set; }
}
