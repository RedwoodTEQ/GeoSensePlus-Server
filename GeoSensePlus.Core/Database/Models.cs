using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Core.Database.Models
{
    public class Tenant
    {
        public string TenantId { get; set; }
    }

    public class CommonProp
    {
        public string Name { get; set; }
        public string Remarks { get; set; }
    }

    public class IndoorTag : CommonProp
    {
        public string TagId { get; set; }
        public string EdgeId { get; set; }

        public Edge Edge { get; set; }
    }

    public class Edge : CommonProp
    {
        public string EdgeId { get; set; }
        public string TenantId { get; set; }

        public Tenant Tenant { get; set; }
        public List<IndoorTag> IndoorTags { get; set; } = new List<IndoorTag>();
    }

    public class IndoorArea : CommonProp
    {
        public string AreaId { get; set; }
        public List<Edge> Edges { get; set; }  = new List<Edge>();
    }

    public class Floor : CommonProp
    {
        public string FloorId { get; set; }

        /// <summary>
        /// Full path of the floorplan image location, which may be a file
        /// system path or a network URI or any other format
        /// </summary>
        public string FloorplanLocation { get; set; }

        public List<IndoorArea> IndoorAreas { get; set; } = new List<IndoorArea>();
    }
}
