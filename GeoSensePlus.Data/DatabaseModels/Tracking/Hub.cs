using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Tracking
{
    public class HubEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Renamed from gateway (to distinguish from concept of lora gateway).
    /// Used for collecting local cell id tracking messages via lora and communicating with cloud services.
    /// There is usually only one hub for a building.
    /// </summary>
    public class Hub : HubEntity, IIdAvailable<int>
    {
        public List<CellAnchor> CellAnchors { get; set; } = new List<CellAnchor>();
        public List<CellTag> CellTags { get; set; } = new List<CellTag>();
        public int GatewayId { get; set; }

        public int GetId()
        {
            return GatewayId;
        }
    }
}
