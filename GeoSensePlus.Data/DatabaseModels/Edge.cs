using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class EdgeEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Edge : EdgeEntity, IIdAvailable<int>
    {
        public Area Area { get; set; }
        public Gateway Gateway { get; set; }
        public List<CellTag> CellTags { get; set; } = new List<CellTag>();

        public int EdgeId { get; set; }

        public int GetId()
        {
            return EdgeId;
        }
    }
}
