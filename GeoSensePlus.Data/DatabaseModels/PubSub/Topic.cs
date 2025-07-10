using Microsoft.EntityFrameworkCore;

namespace GeoSensePlus.Data.DatabaseModels.PubSub;

public class TopicEntity : NamedEntity<int>
{
    // This class is used for API requests, so it does not need any additional properties.
}

/// <summary>
/// Subscription topic
/// Used by MQTT or any other notification engines
/// </summary>
[Index(nameof(Name), IsUnique = true)]
public class Topic : TopicEntity
{
}
