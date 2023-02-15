using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Tracking
{
    public class CellTagEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    /// <summary>
    /// A cell tag is a combination of a BLE reader and a lora gateway, it
    /// reads BLE beacon signal nearby and transmit the location messages via
    /// lora to a hub in the local building.
    /// </summary>
    public class CellTag : CellTagEntity, IIdAvailable<int>
    {
        public Target Target { get; set; }

        public int CellTagId { get; set; }

        public int GetId()
        {
            return CellTagId;
        }
    }
}
