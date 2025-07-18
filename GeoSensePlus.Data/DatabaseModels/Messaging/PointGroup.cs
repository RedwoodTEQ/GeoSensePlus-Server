using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GeoSensePlus.Data.DatabaseModels.Messaging;

[Index(nameof(ParentId))]
public class PointGroupEntity : NamedEntity<int>
{
    [Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;   // if don't want to cascade delete all children, just set this to true

    [Column("parent_id")]
    public int? ParentId { get; set; }
}

/// <summary>
/// UNS point group
/// </summary>
[Table("point_group", Schema = SchemaNames.messaging)]
[Index(nameof(Name), nameof(ParentId), IsUnique = true)]
public class PointGroup : PointGroupEntity
{
    [JsonIgnore]    // ignore in web response serialization, to avoid circular references
    public PointGroup? Parent { get; set; }

    public List<PointGroup> Children { get; set; } = new List<PointGroup>();
    public List<Point> Points { get; set; } = new List<Point>();

}
