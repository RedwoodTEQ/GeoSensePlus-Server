using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class EdgeEntity
    {
        public int EdgeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Edge : EdgeEntity
    {
        public Area Area { get; set; }
        public Gateway Gateway { get; set; }
        public List<IndoorTag> BleTags { get; set; } = new List<IndoorTag>();
    }
}
