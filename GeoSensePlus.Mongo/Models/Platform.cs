namespace GeoSensePlus.Mongo.Models.Platform
{
    public class Channel : ModelBase
    {
    }

    /// <summary>
    /// non-site services
    /// </summary>
    public class Service : ModelBase
    {
        public string Type { get; set; }
    }

    /// <summary>
    /// A remote gsv service
    /// </summary>
    public class Site : ModelBase
    {
    }

    public class Network : ModelBase
    {
    }
}
