using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.Tracking;

public class GpsTagEntity : NamedEntity<int>
{
    [Column("longitude")]
    public double Longitude { get; set; }

    [Column("latitude")]
    public double Latitude { get; set; }

    [Column("altitude")]
    public double Altitude { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("target_id")]
    public int? TargetId { get; set; } // Foreign key to Target
}

[Table("gps_tag", Schema = SchemaNames.tracking)]
public class GpsTag : GpsTagEntity
{
    public Target Target { get; set; }
}
