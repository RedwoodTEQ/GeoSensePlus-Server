using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GeoSensePlus.Data.DatabaseModels.Messaging;

[Index(nameof(ParentId))]
public class PointGroupEntity : BaseEntity<int>
{
    [Column("parent_id")]
    public int? ParentId { get; set; }
}

/// <summary>
/// UNS point group.
/// 
/// NOTE:
/// A unique index of (name, parent_id) with "NULLS NOT DISTINCT" is add in the migration.
/// </summary>
[Table("point_group", Schema = SchemaNames.messaging)]
public class PointGroup : PointGroupEntity
{
    [JsonIgnore]    // ignore in web response serialization, to avoid circular references
    public PointGroup Parent { get; set; }

    public List<PointGroup> Children { get; set; } = new List<PointGroup>();
    public List<Point> Points { get; set; } = new List<Point>();

}
