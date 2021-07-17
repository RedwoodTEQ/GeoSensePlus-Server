using GeoSensePlus.Data.DatabaseModels;
using GeoSensePlus.WebApi.Controllers.Base;
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
    public class AreasController : ControllerBase
    {
        IRepository<Area> _repo;
        IRepositoryController<Area> _controller;

        public AreasController(IRepository<Area> repoArea)
        {
            _repo = repoArea;
            _controller = new RepositoryController<Area>(_repo);
        }

        // GET: api/Areas
        [HttpGet]
        public IEnumerable<Area> Get()
        {
            return _controller.Get();
        }

        // GET api/Areas/5
        [HttpGet("{id}")]
        public ActionResult<Area> Get(int id)
        {
            return _controller.Get(id);
        }

        // POST api/Areas
        [HttpPost]
        public ActionResult<AreaEntity> Post([FromBody] AreaEntity value)
        {
            var area = new Area { Name = value.Name, Description = value.Description, EdgeId = value.EdgeId};
            return _controller.Post(area);
        }

        // PUT api/Areas/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] AreaEntity value)
        {
            var area = new Area {AreaId = id, Name = value.Name, Description = value.Description, EdgeId = value.EdgeId};
            return _controller.Put(area);
        }

        // DELETE api/Areas/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controller.Delete(id);
        }
    }
}
