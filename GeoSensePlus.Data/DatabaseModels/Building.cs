using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class BuildingEntity : IIdAvailable<int>
    {
        public int BuildingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int GetId()
        {
            return BuildingId;
        }
    }

    public class Building : BuildingEntity
    {
        public List<Floorplan> Floorplans { get; set; } = new List<Floorplan>();
    }
}
