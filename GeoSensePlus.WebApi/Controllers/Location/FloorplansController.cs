using GeoSensePlus.Data.DatabaseModels.Location;
using GeoSensePlus.Data.DatabaseModels.Sensing;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Location
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloorplansController : ControllerBase
    {
        IControllerUtil<FloorPlan> _controllerUtil;

        public FloorplansController(IControllerUtil<FloorPlan> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<FloorPlan> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<FloorPlan> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<FloorPlanEntity> Post([FromBody] FloorPlanEntity value)
        {
            var floorplan = new FloorPlan
            {
                Name = value.Name,
                Description = value.Description
            };
            return _controllerUtil.Post(floorplan);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] MeasureEntity value)
        {
            var floorplan = new FloorPlan
            {
                Id = id,
                Name = value.Name,
                Description = value.Description
            };
            return _controllerUtil.Put(floorplan);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
