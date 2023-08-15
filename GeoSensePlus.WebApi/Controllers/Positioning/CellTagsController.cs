using GeoSensePlus.Data.DatabaseModels.Tracking;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Positioning
{
    [Route("api/[controller]")]
    [ApiController]
    public class CellTagsController : ControllerBase
    {
        IControllerUtil<CellTag> _controllerUtil;

        public CellTagsController(IControllerUtil<CellTag> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<CellTag> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<CellTag> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<CellTagEntity> Post([FromBody] CellTagEntity value)
        {
            var celltag = new CellTag { Name = value.Name, Description = value.Description };
            return _controllerUtil.Post(celltag);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CellTagEntity value)
        {
            var celltag = new CellTag { CellTagId = id, Name = value.Name, Description = value.Description };
            return _controllerUtil.Put(celltag);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
