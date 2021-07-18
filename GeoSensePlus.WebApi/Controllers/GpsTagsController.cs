using GeoSensePlus.Data.DatabaseModels;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GpsTagsController: ControllerBase
    {
        IControllerUtil<GpsTag> _controllerUtil;

        public GpsTagsController(IControllerUtil<GpsTag> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<GpsTag> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<GpsTag> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<GpsTagEntity> Post([FromBody] GpsTagEntity value)
        {
            var tag = new GpsTag { Name = value.Name, Description = value.Description };
            return _controllerUtil.Post(tag);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] GpsTagEntity value)
        {
            var tag = new GpsTag {GpsTagId = id, Name = value.Name, Description = value.Description };
            return _controllerUtil.Put(tag);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }

    }
}
