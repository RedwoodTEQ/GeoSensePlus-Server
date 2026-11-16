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

    [Column("point_id")]
    public int PointId { get; set; }
}

/// <summary>
/// A channel is a message source (inbound) or target (outbound), e.g. a MQ server, blockchain, ROS, TTN etc.
/// </summary>
[Table("channel", Schema = SchemaNames.messaging)]
public class Channel : ChannelEntity
{
    public Point Point { get; set; } = null!;
}

public class ChannelConfiguration : BaseEntityConfiguration<Channel, int>
{
    public override void Configure(EntityTypeBuilder<Channel> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Configuration).HasColumnType("jsonb").IsRequired();
    }
}
