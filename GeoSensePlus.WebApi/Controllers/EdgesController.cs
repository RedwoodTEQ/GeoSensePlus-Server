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
    public class EdgesController : ControllerBase
    {
        IControllerUtil<Edge> _controllerUtil;

        public EdgesController(IControllerUtil<Edge> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<Edge> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<Edge> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<EdgeEntity> Post([FromBody] EdgeEntity value)
        {
            var area = new Edge{ Name = value.Name, Description = value.Description };
            return _controllerUtil.Post(area);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] EdgeEntity value)
        {
            var edge = new Edge { EdgeId = id, Name = value.Name, Description = value.Description };
            return _controllerUtil.Put(edge);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
