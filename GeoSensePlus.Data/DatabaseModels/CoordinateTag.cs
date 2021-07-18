using GeoSensePlus.Data.DatabaseModels.Base;
using System;

namespace GeoSensePlus.Data.DatabaseModels
{

    public class CoordinateTagEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public double AxisX { get; set; } = -1;
        public double AxisY { get; set; } = -1;
        public double AxisZ { get; set; } = -1;
        public DateTime TimeStamp { get; set; }
    }

    public class CoordinateTag : CoordinateTagEntity, IIdAvailable<int>
    {
        public Target Target { get; set; }

        public int CoordinateTagId { get; set; }

        public int GetId()
        {
            return CoordinateTagId;
        }
    }
}
