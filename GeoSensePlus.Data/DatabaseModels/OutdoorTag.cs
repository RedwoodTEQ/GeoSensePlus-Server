using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class OutdoorTagEntity
    {
        public int OutdoorTagId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class OutdoorTag : OutdoorTagEntity
    {
        public Target Target { get; set; }
    }
}
