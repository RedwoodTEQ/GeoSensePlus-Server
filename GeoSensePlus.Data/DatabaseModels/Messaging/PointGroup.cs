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
/// A unique index of (name, parent_id) with "NULLS NOT DISTINCT" is defined in PointGroupConfiguration.
/// </summary>
[Table("point_group", Schema = SchemaNames.messaging)]
public class PointGroup : PointGroupEntity
{
    [JsonIgnore]    // ignore in web response serialization, to avoid circular references
    public PointGroup Parent { get; set; }

    public List<PointGroup> Children { get; set; } = new List<PointGroup>();
    public List<DataPoint> DataPoints { get; set; } = new List<DataPoint>();
}

public class PointGroupConfiguration : BaseEntityConfiguration<PointGroup, int>
{
    public override void Configure(EntityTypeBuilder<PointGroup> builder)
    {
        base.Configure(builder);

        builder.HasIndex(p => new { p.Name, p.ParentId })
            .IsUnique().AreNullsDistinct(false)   // Apply "NULLs NOT DISTINCT" (for PostgreSQL v15+) - Two same names with null parent shouldn't be allowed
            .HasDatabaseName($"idx{nameof(PointGroup)}{nameof(PointGroup.Name)}{nameof(PointGroup.ParentId)}".ToSnakeCase());   // idx_point_group_name_parent_id

        builder.HasIndex(g => g.ParentId)
            .HasDatabaseName($"idx{nameof(PointGroup)}{nameof(PointGroup.ParentId)}".ToSnakeCase());   // idx_point_group_parent_id

        builder.HasOne(g => g.Parent)
            .WithMany(g => g.Children)
            .HasForeignKey(g => g.ParentId)
            .HasConstraintName($"fk{nameof(PointGroup)}{nameof(PointGroup.ParentId)}".ToSnakeCase())   // fk_point_group_parent_id
            .OnDelete(DeleteBehavior.Restrict);
    }
}
