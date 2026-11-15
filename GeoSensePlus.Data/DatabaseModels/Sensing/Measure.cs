using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.Sensing;

public class MeasureEntity : BaseEntity<int>
{
    /// <summary>
    /// Multiple lables are separated by ';'
    /// Used as influxdb tags
    /// </summary>
    [Column("labels")]
    public string Labels { get; set; }
}

[Obsolete("This class is deprecated and will be removed.")]     // _todo: Remove the Measure class
[Table("measure", Schema = SchemaNames.sensing)]
public class Measure : MeasureEntity
{
    //public Area CachedArea { get; set; }
    public List<Sensor> Sensors { get; set; } = new List<Sensor>();
}
