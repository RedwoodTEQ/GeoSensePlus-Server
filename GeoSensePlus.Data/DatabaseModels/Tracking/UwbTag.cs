using System;

namespace GeoSensePlus.Data.DatabaseModels.Tracking;

public class UwbTagEntity : NamedEntity<int>
{
    public double AxisX { get; set; } = -1;
    public double AxisY { get; set; } = -1;
    public double AxisZ { get; set; } = -1;
    public DateTime TimeStamp { get; set; }

    public int TargetId { get; set; } // Foreign key to Target
}

public class UwbTag : UwbTagEntity
{
    public Target Target { get; set; }
}
