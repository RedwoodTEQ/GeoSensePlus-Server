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
    /// A cell tag is just a BLE beacon, which sends out BLE signals
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
