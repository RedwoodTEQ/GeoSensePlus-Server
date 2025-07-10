using GeoSensePlus.Data.DatabaseModels.Location;
using GeoSensePlus.Data.DatabaseModels.Tracking;
using System;

namespace GeoSensePlus.Data.DatabaseModels.Sensing;

public class SensorEntity: NamedEntity<long>
{
    /// <summary>
    /// Temperature, humidity etc.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Multiple lables are separated by ';'
    /// Used as influxdb tags
    /// </summary>
    public string Labels { get; set; }

    /// <summary>
    /// The latest reported value
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// Meter, Celsius, etc.
    /// </summary>
    public string Unit { get; set; }

    public DateTime TimeStamp { get; set; }
}

public class Sensor: SensorEntity
{
    //public Area CachedArea { get; set; }
    public Measure Measure { get; set; }
    public Area Area { get; set; }
}
