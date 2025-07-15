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
    public class RemoteGsvController : MongoController<RemoteGsv>
    {
        public RemoteGsvController(IMongoDocReader<RemoteGsv> reader, IMongoDocWriter<RemoteGsv> writer) : base(reader, writer)
        { }
    }
}
