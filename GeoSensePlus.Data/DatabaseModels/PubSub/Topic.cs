using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.PubSub;

public class TopicEntity : NamedEntity<int>
{
    // This class is used for API requests, so it does not need any additional properties.
}

/// <summary>
/// Subscription topic
/// Used by MQTT or any other notification engines
/// </summary>
[Table("topic", Schema = SchemaNames.map)]
[Index(nameof(Name), IsUnique = true)]
public class Topic : TopicEntity
{
}
