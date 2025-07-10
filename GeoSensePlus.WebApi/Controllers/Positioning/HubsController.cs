using GeoSensePlus.Data.DatabaseModels.Tracking;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Positioning
{
    [Route("api/[controller]")]
    [ApiController]
    public class HubsController : ControllerBase
    {
        IControllerUtil<CellHub> _controllerUtil;

        public HubsController(IControllerUtil<CellHub> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<CellHub> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<CellHub> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<CellHubEntity> Post([FromBody] CellHubEntity value)
        {
            var gateway = new CellHub { Name = value.Name, Description = value.Description };
            return _controllerUtil.Post(gateway);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CellHubEntity value)
        {
            var gateway = new CellHub { Id = id, Name = value.Name, Description = value.Description };
            return _controllerUtil.Put(gateway);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
