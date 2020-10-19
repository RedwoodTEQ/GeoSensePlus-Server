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
    public class SitesController : MongoController<Site>
    {
        public SitesController(IMongoDocReader<Site> reader, IMongoDocWriter<Site> writer) : base(reader, writer)
        { }
    }
}
