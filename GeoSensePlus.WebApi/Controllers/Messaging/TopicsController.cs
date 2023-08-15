using GeoSensePlus.Data.DatabaseModels;
using GeoSensePlus.Data.DatabaseModels.Tracking;
using GeoSensePlus.Mqtt;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Messaging
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        IControllerUtil<Topic> _controllerUtil;
        IMqttService _mqttSvc;

        public TopicsController(IControllerUtil<Topic> controllerUtil, IMqttService mqttSvc)
        {
            _controllerUtil = controllerUtil;
            _mqttSvc = mqttSvc;
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

        [HttpPost("data")]
        public async Task<IActionResult> Test(int data)
        {
            await _mqttSvc.PublishAsync("RawMessage", data.ToString());
            return NoContent();
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
