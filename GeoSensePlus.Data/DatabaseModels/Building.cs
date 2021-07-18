using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class BuildingEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Building : BuildingEntity, IIdAvailable<int>
    {
        public List<Floorplan> Floorplans { get; set; } = new List<Floorplan>();

        public int BuildingId { get; set; }

        public int GetId()
        {
            return BuildingId;
        }
    }
}
