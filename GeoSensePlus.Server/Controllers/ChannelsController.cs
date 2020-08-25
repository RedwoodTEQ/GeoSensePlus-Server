using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoSensePlus.Mongo;
using Microsoft.AspNetCore.Mvc;
using NetCoreUtils.Database.MongoDb;

namespace GeoSensePlus.Server.Controllers
{
    public class MongoController<TDoc> : ControllerBase where TDoc : MongoDoc
    {
        IMongoDocReader<TDoc> _reader;

        public MongoController(IMongoDocReader<TDoc> reader)
        {
            _reader = reader;
        }

        [HttpGet]
        public ActionResult<List<TDoc>> Get()
        {
            return _reader.Find(x => true);
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ChannelsController : MongoController<Channel>
    {
        public ChannelsController(IMongoDocReader<Channel> reader) : base(reader)
        { }
    }
}
