using GeoSensePlus.Data.DatabaseModels;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        IControllerUtil<Topic> _controllerUtil;

        public TopicsController(IControllerUtil<Topic> controllerUtil)
        {
            _controllerUtil = controllerUtil;
        }

        [HttpGet]
        public IEnumerable<Topic> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<Topic> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<TopicEntity> Post([FromBody] TopicEntity value)
        {
            var topic = new Topic { Name = value.Name, Description = value.Description };
            return _controllerUtil.Post(topic);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] TargetEntity value)
        {
            var topic = new Topic { TopicId = id, Name = value.Name, Description = value.Description };
            return _controllerUtil.Put(topic);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
