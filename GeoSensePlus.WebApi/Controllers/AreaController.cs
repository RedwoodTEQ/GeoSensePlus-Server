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
        IRepository<Area> _repoArea;

        public AreaController(IRepository<Area> repoArea)
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
        [HttpGet("{id}")]
        public ActionResult<Area> Get(int id)
        {
            var result = _repoArea.Get(id);
            if (result is null)
                return NotFound();
            return result;
        }

        // POST api/<AreaController>
        [HttpPost]
        public ActionResult<AreaEntity> Post([FromBody] AreaEntity areaEntity)
        {
            var area = new Area(areaEntity);
            _repoArea.Add(area);
            _repoArea.Commit();

            return CreatedAtAction(nameof(Get), new { id = area.AreaId }, area);
        }

        //// PUT api/<AreaController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AreaController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _repoArea.Remove(a => a.AreaId == id);
            _repoArea.Commit();
            return NoContent();
        }
    }
}
