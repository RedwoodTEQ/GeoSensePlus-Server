using System;

namespace GeoSensePlus.Data.DatabaseModels.Tracking;

public class GpsTagEntity : NamedEntity<int>
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public double Altitude { get; set; }
    public DateTime TimeStamp { get; set; }

    public int TargetId { get; set; } // Foreign key to Target
}

public class GpsTag : GpsTagEntity
{
    public Target Target { get; set; }
}
