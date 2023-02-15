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
    /// Renamed from "edge"
    /// 
    /// Currently, a cell anchor is just a BLE beacon, named to "anchor" to
    /// keep consistant with other tracking devices' naming.
    /// </summary>
    public class CellAnchor : CellAnchorEntity, IIdAvailable<int>
    {
        public Zone Area { get; set; }
        public Hub Gateway { get; set; }
        public List<CellTag> CellTags { get; set; } = new List<CellTag>();

        public int EdgeId { get; set; }

        public int GetId()
        {
            return EdgeId;
        }
    }
}
