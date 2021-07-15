using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class IndoorTagEntity
    {
        public int IndoorTagId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class IndoorTag : IndoorTagEntity
    {
        public Target Target { get; set; }
    }
}
