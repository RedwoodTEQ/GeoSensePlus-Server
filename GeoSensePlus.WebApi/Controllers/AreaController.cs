using GeoSensePlus.Data.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using NetCoreUtils.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        Repository<Area> _repoArea;

        public AreaController(Repository<Area> repoArea)
        {
            _repoArea = repoArea;
        }

        // GET: api/<AreaController>
        [HttpGet]
        public IEnumerable<Area> Get()
        {
            return _repoArea.GetAllNoTracking();
        }

        // GET api/<AreaController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<AreaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _repoArea.Add(new Area { Name = value });
            _repoArea.Commit();

            //return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());
        }

        //// PUT api/<AreaController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AreaController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
