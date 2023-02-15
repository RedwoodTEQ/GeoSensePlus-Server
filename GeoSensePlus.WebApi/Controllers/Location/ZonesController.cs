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
        IControllerUtil<Zone> _controllerUtil;

        public ZonesController(IControllerUtil<Zone> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<Zone> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<Zone> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<ZoneEntity> Post([FromBody] ZoneEntity value)
        {
            var area = new Zone { Name = value.Name, Description = value.Description, CellAnchorId = value.CellAnchorId };
            return _controllerUtil.Post(area);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ZoneEntity value)
        {
            var area = new Zone { AreaId = id, Name = value.Name, Description = value.Description, CellAnchorId = value.CellAnchorId };
            return _controllerUtil.Put(area);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
