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
    public class EdgesController : ControllerBase
    {
        IRepository<Edge> _repo;

        public EdgesController(IRepository<Edge> repoEdge)
        {
            _repo = repoEdge;
        }

        // GET: api/<EdgeController>
        [HttpGet]
        public IEnumerable<Edge> Get()
        {
            return _repo.GetAllNoTracking();
        }

        // GET api/<EdgeController>/5
        [HttpGet("{id}")]
        public ActionResult<Edge> Get(int id)
        {
            var result = _repo.Get(id);
            if (result is null)
                return NotFound();
            return result;
        }

        // POST api/<EdgeController>
        [HttpPost]
        public ActionResult<EdgeEntity> Post([FromBody] EdgeEntity value)
        {
            var edge = new Edge { Name = value.Name, Description = value.Description};
            _repo.Add(edge);
            _repo.Commit();

            return CreatedAtAction(nameof(Get), new { id = edge.EdgeId }, edge);
        }

        // PUT api/<EdgeController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<EdgeController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _repo.Remove(i => i.EdgeId == id);
            _repo.Commit();
            return NoContent();
        }
    }
}
