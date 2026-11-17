using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GeoSensePlus.Data.DatabaseModels.Messaging;

public class PointEntity : BaseEntity<int>
{
    /// <summary>
    /// Group ID, must be set, at least set to root group
    /// </summary>
    [Required]
    [Column("parent_id")]
    public int ParentId { get; set; }  // foreign key to PointGroup

    // Current Value
    [Column("scalar_value")]    // will add a VectorValue column later
    public double? ScalarValue { get; set; }
}

/// <summary>
/// An UNS data point. A point contains a final value's data type, which is
/// different from the income value's data type defined in a message channel.
/// 
/// A point must belong to a point group.
/// </summary>
[Table("point", Schema = SchemaNames.messaging)]
public class Point : PointEntity
{
    [JsonIgnore]    // ignore in web response serialization, to avoid circular references
    public PointGroup Parent { get; set; } = null!;
    public List<Channel> Channels { get; set; } = new List<Channel>();  // may have multiple mqtt topics
}

public class PointConfiguration : BaseEntityConfiguration<Point, int>
{
    public override void Configure(EntityTypeBuilder<Point> builder)
    {
        base.Configure(builder);

        builder.HasIndex(p => new { p.Name, p.ParentId }).HasDatabaseName("idx_point_name_parent_id").IsUnique();
        builder.HasOne(i => i.Parent).WithMany(g => g.Points).HasForeignKey(i => i.ParentId).OnDelete(DeleteBehavior.Cascade);
    }
}
