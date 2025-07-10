using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Location;

public class BuildingEntity : NamedEntity<int>
{
}

public class Building : BuildingEntity
{
    public List<FloorPlan> FloorPlans { get; set; } = new List<FloorPlan>();
}
