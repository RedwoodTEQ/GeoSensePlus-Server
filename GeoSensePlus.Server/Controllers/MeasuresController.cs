﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoSensePlus.Mongo;
using GeoSensePlus.Server.Controllers.Base;
using NetCoreUtils.Database.MongoDb;

namespace GeoSensePlus.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasuresController : MongoController<Measure>
    {
        public MeasuresController(IMongoDocReader<Measure> reader, IMongoDocWriter<Measure> writer) : base(reader, writer)
        { }
    }
}
