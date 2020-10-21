using System.Linq;
using GeoSensePlus.Mongo.Models.Platform;
using GeoSensePlus.Server.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using NetCoreUtils.Database.MongoDb;

namespace GeoSensePlus.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ChannelsController : MongoController<Channel>
    {
        public ChannelsController(IMongoDocReader<Channel> reader, IMongoDocWriter<Channel> writer) : base(reader, writer)
        { }
    }
}
