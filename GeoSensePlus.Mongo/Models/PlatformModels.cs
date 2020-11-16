namespace GeoSensePlus.Mongo.Models.Platform
{
    public class Channel : ModelBase
    {
        public string Type { get; set; }
        public string Location { get; set; }
        public bool Active { get; set; }
    }

    /// <summary>
    /// non-site services
    /// </summary>
    public class Service : ModelBase
    {
        public string Type { get; set; }
        public string Location { get; set; }     // the swagger page url
        public bool Active { get; set; }
    }

    /// <summary>
    /// A remote gsv service
    /// </summary>
    public class Site : ModelBase
    {
        public string Location { get; set; }     // the swagger page url
        public bool Active { get; set; }
    }

    public class Network : ModelBase
    {
    }
}
