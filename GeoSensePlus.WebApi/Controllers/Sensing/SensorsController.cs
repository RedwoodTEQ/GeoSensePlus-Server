using GeoSensePlus.Data.DatabaseModels;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Sensing
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        IControllerUtil<Sensor> _controllerUtil;

        public SensorsController(IControllerUtil<Sensor> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<Sensor> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<Sensor> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<SensorEntity> Post([FromBody] SensorEntity value)
        {
            var sensor = new Sensor
            {
                Name = value.Name,
                Description = value.Description,
                Labels = value.Labels,
                Type = value.Type,
                Unit = value.Unit,
                Value = value.Value,
                TimeStamp = value.TimeStamp
            };

            return _controllerUtil.Post(sensor);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] SensorEntity value)
        {
            var sensor = new Sensor
            {
                SensorId = id,
                Name = value.Name,
                Description = value.Description,
                Labels = value.Labels,
                Type = value.Type,
                Unit = value.Unit,
                Value = value.Value,
                TimeStamp = value.TimeStamp
            };
            return _controllerUtil.Put(sensor);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
