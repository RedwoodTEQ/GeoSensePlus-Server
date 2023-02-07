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
    public class AreasController : ControllerBase
    {
        IControllerUtil<Area> _controllerUtil;

        public AreasController(IControllerUtil<Area> controllerUtil)
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
            var area = new Area { Name = value.Name, Description = value.Description, EdgeId = value.EdgeId };
            return _controllerUtil.Post(area);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] AreaEntity value)
        {
            var area = new Area { AreaId = id, Name = value.Name, Description = value.Description, EdgeId = value.EdgeId };
            return _controllerUtil.Put(area);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
