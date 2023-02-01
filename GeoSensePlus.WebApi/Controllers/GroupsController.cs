using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoSensePlus.Mongo.Models;
using GeoSensePlus.Mongo.Models.Sensing;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using NetCoreUtils.Database.MongoDb;

namespace GeoSensePlus.WebApi.Controllers
{
    /// <summary>
    /// A group is similar to an "application" in TTN or ChirpStack, or an organization in influxdb
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : MongoController<Group>
    {
        public GroupsController(IMongoDocReader<Group> reader, IMongoDocWriter<Group> writer) : base(reader, writer)
        { }
    }
}
