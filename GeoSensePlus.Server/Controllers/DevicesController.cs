using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoSensePlus.Mongo.Models;
using GeoSensePlus.Server.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using NetCoreUtils.Database.MongoDb;

namespace GeoSensePlus.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : MongoController<Device>
    {
        public DevicesController(IMongoDocReader<Device> reader, IMongoDocWriter<Device> writer) : base(reader, writer)
        { }
    }
}
