using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GeoSensePlus.Data.DatabaseModels.Messaging;

public class DataPointEntity : BaseEntity<int>
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
/// An UNS data point. A data point contains a final value's data type, which is
/// different from the income value's data type defined in a message channel.
///
/// A data point must belong to a point group.
/// </summary>
[Table("data_point", Schema = SchemaNames.messaging)]
public class DataPoint : DataPointEntity
{
    [JsonIgnore]    // ignore in web response serialization, to avoid circular references
    public PointGroup Parent { get; set; } = null!;
    public List<DataChannel> DataChannels { get; set; } = new List<DataChannel>();  // may have multiple mqtt topics
}

public class DataPointConfiguration : BaseEntityConfiguration<DataPoint, int>
{
    public override void Configure(EntityTypeBuilder<DataPoint> builder)
    {
        base.Configure(builder);

        builder.HasIndex(p => new { p.Name, p.ParentId })
            .HasDatabaseName($"idx{nameof(DataPoint)}{nameof(DataPoint.Name)}{nameof(DataPoint.ParentId)}".ToSnakeCase())   // idx_data_point_name_parent_id
            .IsUnique();

        builder.HasOne(i => i.Parent)
            .WithMany(g => g.DataPoints)
            .HasForeignKey(i => i.ParentId)
            .HasConstraintName($"fk{nameof(DataPoint)}{nameof(DataPoint.ParentId)}".ToSnakeCase())   // fk_data_point_parent_id
            .OnDelete(DeleteBehavior.Cascade);
    }
}
