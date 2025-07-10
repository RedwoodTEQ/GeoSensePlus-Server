using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.Tracking;

public class UwbTagEntity : NamedEntity<int>
{
    [Column("axis_x")]
    public double AxisX { get; set; } = -1;

    [Column("axis_y")]
    public double AxisY { get; set; } = -1;

    [Column("axis_z")]
    public double AxisZ { get; set; } = -1;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("target_id")]
    public int TargetId { get; set; } // Foreign key to Target
}

public class UwbTag : UwbTagEntity
{
    public Target Target { get; set; }
}
