using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.Messaging;

public class PointEntity : NamedEntity<int>
{
    /// <summary>
    /// Group ID, must be set, at least set to root group
    /// </summary>
    [Required]
    [Column("parent_id")]
    public int ParentId { get; set; }  // foreign key to PointGroup
}

/// <summary>
/// An UNS data point
/// </summary>
[Table("point", Schema = SchemaNames.messaging)]
public class Point : PointEntity
{
    public PointGroup Parent { get; set; } = null!;
}
