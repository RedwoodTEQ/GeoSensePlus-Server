using System.Collections.Generic;

namespace GeoSensePlus.Data.DatabaseModels.Sensing;

public class MeasureEntity : NamedEntity<int>
{
    /// <summary>
    /// Multiple lables are separated by ';'
    /// Used as influxdb tags
    /// </summary>
    public string Labels { get; set; }
}

public class Measure : MeasureEntity
{
    //public Area CachedArea { get; set; }
    public List<Sensor> Sensors { get; set; } = new List<Sensor>();
}
