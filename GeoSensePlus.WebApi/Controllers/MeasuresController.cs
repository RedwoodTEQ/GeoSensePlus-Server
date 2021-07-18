using GeoSensePlus.Data.DatabaseModels;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasuresController : ControllerBase
    {
        IControllerUtil<Measure> _controllerUtil;

        public MeasuresController(IControllerUtil<Measure> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<Measure> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<Measure> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<MeasureEntity> Post([FromBody] MeasureEntity value)
        {
            var measure = new Measure { Name = value.Name, Description = value.Description, Labels = value.Labels };
            return _controllerUtil.Post(measure);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] MeasureEntity value)
        {
            var measure = new Measure {MeasureId = id, Name = value.Name, Description = value.Description, Labels = value.Labels };
            return _controllerUtil.Put(measure);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }

    }
}
