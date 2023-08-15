using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NetCoreUtils.Database.MongoDb;

namespace GeoSensePlus.WebApi.Controllers.Base
{
    public class MongoController<TDoc> : ControllerBase where TDoc : MongoDoc
    {
        IMongoDocReader<TDoc> _reader;
        IMongoDocWriter<TDoc> _writer;

        public MongoController(IMongoDocReader<TDoc> reader, IMongoDocWriter<TDoc> writer)
        {
            _reader = reader;
            _writer = writer;
        }

        [HttpGet]
        public async Task<ActionResult<List<TDoc>>> Get()
        {
            return await _reader.FindAsync(x => true);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TDoc>> Get(string id)
        {
            return await _reader.FindAsync(id); // TODO: need to handle exception
        }

        [HttpPost]
        public async Task Post([FromBody] TDoc value)
        {
            await _writer.InsertOneAsync(value);
        }

        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] TDoc value)
        {
            value.Id = new ObjectId(id);
            await _writer.ReplaceAsync(d => d.Id.Equals(ObjectId.Parse(id)), value);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _writer.DeleteOneAsync(d => d.Id.Equals(ObjectId.Parse(id))); // TODO: need to handle exception
        }
    }
}
