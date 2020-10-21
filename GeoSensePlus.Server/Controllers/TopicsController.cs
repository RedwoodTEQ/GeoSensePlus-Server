using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoSensePlus.Mongo.Models.Sensing;
using GeoSensePlus.Server.Controllers.Base;
using NetCoreUtils.Database.MongoDb;

namespace GeoSensePlus.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : MongoController<Topic>
    {
        public TopicsController(IMongoDocReader<Topic> reader, IMongoDocWriter<Topic> writer) : base(reader, writer)
        { }
    }
}
