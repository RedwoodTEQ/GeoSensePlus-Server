using GeoSensePlus.Data.DatabaseModels.Tracking;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using NetCoreUtils.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Positioning
{
    [Route("api/[controller]")]
    [ApiController]
    public class CellAnchorsController : ControllerBase
    {
        IControllerUtil<CellAnchor> _controllerUtil;

        public CellAnchorsController(IControllerUtil<CellAnchor> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<CellAnchor> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<CellAnchor> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<CellAnchorEntity> Post([FromBody] CellAnchorEntity value)
        {
            var area = new CellAnchor { Name = value.Name, Description = value.Description };
            return _controllerUtil.Post(area);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CellAnchorEntity value)
        {
            var edge = new CellAnchor { CellAnchorId = id, Name = value.Name, Description = value.Description };
            return _controllerUtil.Put(edge);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
