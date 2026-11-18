using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GeoSensePlus.Data.DatabaseModels.Messaging;

public class DataChannelEntity : BaseEntity<int>
{
    //public MessageDirection Direction { get; set; }

    //public MessageProtocol Protocol { get; set; }

    [Column("configuration")]
    public JsonDocument Configuration { get; set; } = JsonDocument.Parse("{}");

    //public bool Enabled { get; set; } = true;

    [Column("data_point_id")]
    public int DataPointId { get; set; }
}

/// <summary>
/// A data channel is a message source (inbound) or target (outbound), e.g. a MQ server, blockchain, ROS, TTN etc.
/// </summary>
[Table("data_channel", Schema = SchemaNames.messaging)]
public class DataChannel : DataChannelEntity
{
    public DataPoint DataPoint { get; set; } = null!;
}

public class DataChannelConfiguration : BaseEntityConfiguration<DataChannel, int>
{
    public override void Configure(EntityTypeBuilder<DataChannel> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Configuration).HasColumnType("jsonb").IsRequired();

        builder.HasOne(c => c.DataPoint)
            .WithMany(p => p.DataChannels)
            .HasForeignKey(c => c.DataPointId)
            .HasConstraintName($"fk{nameof(DataChannel)}{nameof(DataChannel.DataPointId)}".ToSnakeCase())   // fk_data_channel_data_point_id
            .OnDelete(DeleteBehavior.Cascade);
    }
}
