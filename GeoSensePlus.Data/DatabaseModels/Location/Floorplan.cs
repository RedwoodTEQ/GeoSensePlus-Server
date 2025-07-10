using System.Collections.Generic;

namespace GeoSensePlus.Data.DatabaseModels.Location;

public class FloorPlanEntity : NamedEntity<int>
{
    public string FileLocation { get; set; } // Path to the floor plan image or file
    public string FloorId { get; set; } // Foreign key to Floor
}

public class FloorPlan : FloorPlanEntity
{
    public Floor Floor { get; set; }
    public List<Area> Areas { get; set; } = new List<Area>();
}
