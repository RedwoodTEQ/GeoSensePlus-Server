using GeoSensePlus.Data.DatabaseModels;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Position
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingsController : ControllerBase
    {
        IControllerUtil<Building> _controllerUtil;

        public BuildingsController(IControllerUtil<Building> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<Building> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<Building> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<BuildingEntity> Post([FromBody] BuildingEntity value)
        {
            var building = new Building { Name = value.Name, Description = value.Description };
            return _controllerUtil.Post(building);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] BuildingEntity value)
        {
            var building = new Building { BuildingId = id, Name = value.Name, Description = value.Description };
            return _controllerUtil.Put(building);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
