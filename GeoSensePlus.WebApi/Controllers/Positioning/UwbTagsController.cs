using GeoSensePlus.Data.DatabaseModels.Tracking;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Positioning
{
    [Route("api/[controller]")]
    [ApiController]
    public class UwbTagsController : ControllerBase
    {
        IControllerUtil<UwbTag> _controllerUtil;

        public UwbTagsController(IControllerUtil<UwbTag> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<UwbTag> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<UwbTag> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<UwbTagEntity> Post([FromBody] UwbTagEntity value)
        {
            var tag = new UwbTag
            {
                Name = value.Name,
                Description = value.Description,
                TimeStamp = value.TimeStamp,
                AxisX = value.AxisX,
                AxisY = value.AxisY,
                AxisZ = value.AxisZ
            };
            return _controllerUtil.Post(tag);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] UwbTagEntity value)
        {
            var tag = new UwbTag
            {
                UwbTagId = id,
                Name = value.Name,
                Description = value.Description,
                TimeStamp = value.TimeStamp,
                AxisX = value.AxisX,
                AxisY = value.AxisY,
                AxisZ = value.AxisZ
            };
            return _controllerUtil.Put(tag);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
