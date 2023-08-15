using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoSensePlus.Mongo.Models.Platform;
using NetCoreUtils.Database.MongoDb;
using GeoSensePlus.WebApi.Controllers.Base;

namespace GeoSensePlus.WebApi.Controllers.Location
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitesController : MongoController<Site>
    {
        public SitesController(IMongoDocReader<Site> reader, IMongoDocWriter<Site> writer) : base(reader, writer)
        { }
    }
}
