using System;

namespace GeoSensePlus.Data.DatabaseModels
{

    public class CoordinateTagEntity
    {
        public int CoordinateTagId { get; set; }
        public double AxisX { get; set; } = -1;
        public double AxisY { get; set; } = -1;
        public double AxisZ { get; set; } = -1;
        public DateTime TimeStamp { get; set; }
    }

    public class CoordinateTag : CoordinateTagEntity
    {
        public Target Target { get; set; }
    }
}
