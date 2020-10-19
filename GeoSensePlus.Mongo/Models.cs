using MongoDB.Driver;
using NetCoreUtils.Database.MongoDb;
using System;

namespace GeoSensePlus.Mongo.Models
{
    public class ModelBase : MongoDoc
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IdString { get { return base.Id.ToString(); } }
    }

    public class Channel : ModelBase
    {
    }

    public class Device : ModelBase
    {
    }

    public class Label : ModelBase
    {
    }

    public class Metric : ModelBase
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

    public class Topic : ModelBase
    {
    }
}
