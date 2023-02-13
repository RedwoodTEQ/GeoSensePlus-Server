using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Tracking
{
    public class GatewayEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Gateway : GatewayEntity, IIdAvailable<int>
    {
        public List<CellAnchor> CellAnchors { get; set; } = new List<CellAnchor>();

        public int GatewayId { get; set; }

        public int GetId()
        {
            return GatewayId;
        }
    }
}
