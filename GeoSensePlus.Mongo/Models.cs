using MongoDB.Driver;
using NetCoreUtils.Database.MongoDb;
using System;

namespace GeoSensePlus.Mongo
{ 
    public class ModelBase : MongoDoc
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Channel : ModelBase
    {
        static public string CollectionName { get; } = "Channels";
    }

    public class Device : ModelBase
    {
        static public string CollectionName { get; } = "Devices";
    }

    public class Label : ModelBase
    {
        static public string CollectionName { get; } = "Labels";
    }

    public class Measure : ModelBase
    {
        static public string CollectionName { get; } = "Measures";
    }

    // non-site services
    public class Service : ModelBase
    {
        static public string CollectionName { get; } = "Services";
        public string Type { get; set; }
    }

    public class Site : ModelBase
    {
        static public string CollectionName { get; } = "Sites";
    }

    public class Topic : ModelBase
    {
        static public string CollectionName { get; } = "Topics";
    }
}
