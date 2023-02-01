using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoSensePlus.Mongo.Models.Platform;
using NetCoreUtils.Database.MongoDb;
using GeoSensePlus.WebApi.Controllers.Base;

namespace GeoSensePlus.WebApi.Controllers
{
    /// <summary>
    /// Non-site service behind a reverse proxy, e.g. positioning service, alarm service, analytics service, identity service, database service etc.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : MongoController<Service>
    {
        public ServicesController(IMongoDocReader<Service> reader, IMongoDocWriter<Service> writer) : base(reader, writer)
        { }
    }
}
