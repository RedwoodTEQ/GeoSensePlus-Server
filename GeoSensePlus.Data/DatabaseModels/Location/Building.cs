using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Location;

public class BuildingEntity : NamedEntity<int>
{
}

[Table("building", Schema = SchemaNames.location)]
public class Building : BuildingEntity
{
    public List<FloorPlan> FloorPlans { get; set; } = new List<FloorPlan>();
}
