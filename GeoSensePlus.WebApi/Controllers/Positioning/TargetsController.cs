using GeoSensePlus.Data.DatabaseModels.Tracking;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Positioning
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        IControllerUtil<Target> _controllerUtil;

        public TargetsController(IControllerUtil<Target> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<Target> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<Target> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<TargetEntity> Post([FromBody] TargetEntity value)
        {
            var target = new Target { Name = value.Name, Description = value.Description };
            return _controllerUtil.Post(target);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] TargetEntity value)
        {
            var target = new Target { TargetId = id, Name = value.Name, Description = value.Description };
            return _controllerUtil.Put(target);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
