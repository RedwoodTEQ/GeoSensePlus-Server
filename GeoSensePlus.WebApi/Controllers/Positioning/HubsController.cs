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
        IControllerUtil<Hub> _controllerUtil;

        public HubsController(IControllerUtil<Hub> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<Hub> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<Hub> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<HubEntity> Post([FromBody] HubEntity value)
        {
            var gateway = new Hub { Name = value.Name, Description = value.Description };
            return _controllerUtil.Post(gateway);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] HubEntity value)
        {
            var gateway = new Hub { HubId = id, Name = value.Name, Description = value.Description };
            return _controllerUtil.Put(gateway);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
