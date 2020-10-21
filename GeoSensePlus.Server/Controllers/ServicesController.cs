using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoSensePlus.Mongo.Models.Platform;
using GeoSensePlus.Server.Controllers.Base;
using NetCoreUtils.Database.MongoDb;

namespace GeoSensePlus.Server.Controllers
{
    /// <summary>
    /// Non-site service
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : MongoController<Service>
    {
        public ServicesController(IMongoDocReader<Service> reader, IMongoDocWriter<Service> writer) : base(reader, writer)
        { }
    }
}
