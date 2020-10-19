using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoSensePlus.Mongo.Models;
using GeoSensePlus.Server.Controllers.Base;
using NetCoreUtils.Database.MongoDb;

namespace GeoSensePlus.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricsController : MongoController<Metric>
    {
        public MetricsController(IMongoDocReader<Metric> reader, IMongoDocWriter<Metric> writer) : base(reader, writer)
        { }
    }
}
