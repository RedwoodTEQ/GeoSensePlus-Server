using GeoSensePlus.Data.DatabaseModels;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoordinateTagsController : ControllerBase
    {
        IControllerUtil<CoordinateTag> _controllerUtil;

        public CoordinateTagsController(IControllerUtil<CoordinateTag> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<CoordinateTag> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<CoordinateTag> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<CoordinateTagEntity> Post([FromBody] CoordinateTagEntity value)
        {
            var tag = new CoordinateTag { Name = value.Name, Description = value.Description };
            return _controllerUtil.Post(tag);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CoordinateTagEntity value)
        {
            var tag = new CoordinateTag { CoordinateTagId = id, Name = value.Name, Description = value.Description };
            return _controllerUtil.Put(tag);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
