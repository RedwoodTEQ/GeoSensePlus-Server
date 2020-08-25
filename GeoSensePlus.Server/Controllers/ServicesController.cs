using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GeoSensePlus.Server.Controllers
{
    /// <summary>
    /// Non-site service
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        // GET: api/<ServicesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ServicesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ServicesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ServicesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ServicesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
