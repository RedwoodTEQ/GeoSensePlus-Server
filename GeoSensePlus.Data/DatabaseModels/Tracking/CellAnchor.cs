using GeoSensePlus.Data.DatabaseModels.Base;
using GeoSensePlus.Data.DatabaseModels.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Tracking
{
    public class CellAnchorEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Renamed from "edge", to keep consistant with other tracking devices' naming.
    /// 
    /// A cell anchor is a combination of a BLE reader and a lora gateway, it
    /// reads BLE beacon signal nearby and transmit the location messages via
    /// lora to a hub in the local building.
    /// </summary>
    public class CellAnchor : CellAnchorEntity, IIdAvailable<int>
    {
        public Zone Zone { get; set; }
        public Hub Hub { get; set; }
        public List<CellTag> CellTags { get; set; } = new List<CellTag>();

        public int CellAnchorId { get; set; }

        public int GetId()
        {
            return CellAnchorId;
        }
    }
}
