using NetCoreUtils.Database.MongoDb;

namespace GeoSensePlus.Mongo.Models
{
    public class ModelBase : MongoDoc
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IdString { get { return base.Id.ToString(); } }
    }
}
