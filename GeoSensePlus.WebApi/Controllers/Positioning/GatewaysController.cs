using GeoSensePlus.Data.DatabaseModels.Tracking;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Positioning
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewaysController : ControllerBase
    {
        IControllerUtil<Gateway> _controllerUtil;

        public GatewaysController(IControllerUtil<Gateway> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<Gateway> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<Gateway> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<GatewayEntity> Post([FromBody] GatewayEntity value)
        {
            var gateway = new Gateway { Name = value.Name, Description = value.Description };
            return _controllerUtil.Post(gateway);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] GatewayEntity value)
        {
            var gateway = new Gateway { GatewayId = id, Name = value.Name, Description = value.Description };
            return _controllerUtil.Put(gateway);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
