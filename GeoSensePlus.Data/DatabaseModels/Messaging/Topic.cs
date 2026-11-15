using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GeoSensePlus.Data.DatabaseModels.Messaging;

public class TopicEntity : BaseEntity<int>
{
    //public MessageDirection Direction { get; set; }

    //public MessageProtocol Protocol { get; set; }

    [Column("configuration")]
    public JsonDocument Configuration { get; set; } = JsonDocument.Parse("{}");

    //public bool Enabled { get; set; } = true;
}

/// <summary>
/// Subscription topic
/// Used by MQTT or any other notification engines
/// </summary>
[Obsolete("Rename to Channel")]     // _todo: Rename to Channel
//[Table("topic", Schema = SchemaNames.messaging)]
//[Index(nameof(Name), IsUnique = true)]
public class Topic : TopicEntity
{
}

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.ToTable("topic", SchemaNames.messaging);
        builder.HasKey(o => o.Id);
        builder.HasIndex(o => o.Name).IsUnique();
        builder.Property(x => x.Configuration)
            .HasColumnType("jsonb")
            .IsRequired();
    }
}
