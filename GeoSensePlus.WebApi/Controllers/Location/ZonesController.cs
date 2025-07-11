using GeoSensePlus.Data.DatabaseModels.Location;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using NetCoreUtils.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Location
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonesController : ControllerBase
    {
        IControllerUtil<Area> _controllerUtil;

        public ZonesController(IControllerUtil<Area> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<Area> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<Area> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<AreaEntity> Post([FromBody] AreaEntity value)
        {
            var area = new Area { Name = value.Name, Description = value.Description };
            return _controllerUtil.Post(area);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] AreaEntity value)
        {
            var area = new Area { Id = id, Name = value.Name, Description = value.Description };
            return _controllerUtil.Put(area);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
