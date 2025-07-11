using GeoSensePlus.Data.DatabaseModels.Location;
using GeoSensePlus.Data.DatabaseModels.Tracking;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.Sensing;

public class SensorEntity: NamedEntity<long>
{
    /// <summary>
    /// Temperature, humidity etc.
    /// </summary>
    [Column("type")]
    public string Type { get; set; }

    /// <summary>
    /// Multiple lables are separated by ';'
    /// Used as influxdb tags
    /// </summary>
    [Column("labels")]
    public string Labels { get; set; }

    /// <summary>
    /// The latest reported value
    /// </summary>
    [Column("value")]
    public double Value { get; set; }

    /// <summary>
    /// Meter, Celsius, etc.
    /// </summary>
    [Column("unit")]
    public string Unit { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("area_id")]
    public int? AreaId { get; set; } // foreign key to Area

    [Column("measure_id")]
    public int? MeasureId { get; set; } // foreign key to Measure
}

[Table("sensor", Schema = SchemaNames.sensing)]
public class Sensor: SensorEntity
{
    //public Area CachedArea { get; set; }
    public Measure Measure { get; set; }
    public Area Area { get; set; }
}
