using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoSensePlus.Mongo.Models.Platform;
using NetCoreUtils.Database.MongoDb;
using GeoSensePlus.WebApi.Controllers.Base;

namespace GeoSensePlus.WebApi.Controllers.System
{
    /// <summary>
    /// Non-site service behind a reverse proxy, e.g.
    ///     positioning service, 
    ///     alarm service, 
    ///     analytics service, 
    ///     identity service, 
    ///     database service etc.
    /// This controller may not be necessary, may need to be replaced by a microservice framework
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    internal class _ServicesController : MongoController<Service>
    {
        public _ServicesController(IMongoDocReader<Service> reader, IMongoDocWriter<Service> writer) : base(reader, writer)
        { }
    }
}
