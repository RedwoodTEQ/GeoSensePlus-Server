using GeoSensePlus.Data.DatabaseModels;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Locating
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeofencesController : ControllerBase
    {
        IControllerUtil<Geofence> _controllerUtil;

        public GeofencesController(IControllerUtil<Geofence> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<Geofence> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<Geofence> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<GeofenceEntity> Post([FromBody] GeofenceEntity value)
        {
            var geofence = new Geofence { Name = value.Name, Description = value.Description };
            return _controllerUtil.Post(geofence);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] GeofenceEntity value)
        {
            var geofence = new Geofence { GeofenceId = id, Name = value.Name, Description = value.Description };
            return _controllerUtil.Put(geofence);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
