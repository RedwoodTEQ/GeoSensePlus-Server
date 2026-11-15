using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GeoSensePlus.Data.DatabaseModels.Messaging;

public class ChannelEntity : BaseEntity<int>
{
    //public MessageDirection Direction { get; set; }

    //public MessageProtocol Protocol { get; set; }

    [Column("configuration")]
    public JsonDocument Configuration { get; set; } = JsonDocument.Parse("{}");

    //public bool Enabled { get; set; } = true;
}

/// <summary>
/// A channel is a message source (inbound) or target (outbound), e.g. a MQ server, blockchain, ROS, TTN etc.
/// </summary>
public class Channel : ChannelEntity
{
}

public class TopicConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.ToTable("topic", SchemaNames.messaging);
        builder.HasKey(o => o.Id);
        builder.HasIndex(o => o.Name).IsUnique();
        builder.Property(x => x.Configuration)
            .HasColumnType("jsonb")
            .IsRequired();
    }
}
