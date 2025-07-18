using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GeoSensePlus.Data.DatabaseModels.Messaging;

public class PointEntity : NamedEntity<int>
{
    /// <summary>
    /// Group ID, must be set, at least set to root group
    /// </summary>
    [Required]
    [Column("parent_id")]
    public int ParentId { get; set; }  // foreign key to PointGroup

    [Column("value_id")]
    public int? ValueId { get; set; }  // foreign key to Value, can be null if no value is associated
}

/// <summary>
/// An UNS data point. A point can have one value and multiple topics.
/// </summary>
[Table("point", Schema = SchemaNames.messaging)]
[Index(nameof(Name), nameof(ParentId), IsUnique = true)]
public class Point : PointEntity
{
    [JsonIgnore]    // ignore in web response serialization, to avoid circular references
    public PointGroup Parent { get; set; } = null!;
    public Value Value { get; set; } = null!;  // value associated with this point, e.g. "temperature", "humidity", etc.
    public List<Topic> Topics { get; set; } = new List<Topic>();  // may have multiple mqtt topics
}
