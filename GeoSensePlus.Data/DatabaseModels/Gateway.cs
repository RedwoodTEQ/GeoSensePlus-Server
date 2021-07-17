using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class GatewayEntity : IIdAvailable<int>
    {
        public int GatewayId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int GetId()
        {
            return GatewayId;
        }
    }

    public class Gateway : GatewayEntity
    {
        public List<Edge> Edges { get; set; } = new List<Edge>();
    }
}
