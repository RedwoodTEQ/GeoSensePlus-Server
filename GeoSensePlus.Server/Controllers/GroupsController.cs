using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoSensePlus.Mongo.Models;
using GeoSensePlus.Mongo.Models.Sensing;
using GeoSensePlus.Server.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using NetCoreUtils.Database.MongoDb;

namespace GeoSensePlus.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : MongoController<Group>
    {
        public GroupsController(IMongoDocReader<Group> reader, IMongoDocWriter<Group> writer) : base(reader, writer)
        { }
    }
}
