using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeoSensePlus.Data.DatabaseModels.Messaging;

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

public class PointGroupConfiguration : BaseEntityConfiguration<PointGroup, int>
{
    public override void Configure(EntityTypeBuilder<PointGroup> builder)
    {
        base.Configure(builder);

        builder.HasIndex(p => new { p.Name, p.ParentId }).HasDatabaseName("idx_point_name_parent_id").IsUnique();
        builder.HasOne(g => g.Parent).WithMany(g => g.Children).HasForeignKey(g => g.ParentId).OnDelete(DeleteBehavior.Restrict);
    }
}
