using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class CellTagEntity : IIdAvailable<int>
    {
        public int CellTagId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime TimeStamp { get; set; }

        public int GetId()
        {
            return CellTagId;
        }
    }

    public class CellTag : CellTagEntity
    {
        public Target Target { get; set; }
    }
}
