using GeoSensePlus.Data.DatabaseModels.Messaging;
using GeoSensePlus.Data.DatabaseModels.Tracking;
using GeoSensePlus.Mqtt;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Messaging
{
    [Obsolete("Topic has been renamed to DataChannel, use ChannelsController instead")]
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        IControllerUtil<DataChannel> _controllerUtil;
        IMqttService _mqttSvc;

        public TopicsController(IControllerUtil<DataChannel> controllerUtil, IMqttService mqttSvc)
        {
            _controllerUtil = controllerUtil;
            _mqttSvc = mqttSvc;
        }

        [HttpGet]
        public IEnumerable<DataChannel> Get()
        {
            return _controllerUtil.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<DataChannel> Get(int id)
        {
            return _controllerUtil.Get(id);
        }

        [HttpPost]
        public ActionResult<DataChannelEntity> Post([FromBody] DataChannelEntity value)
        {
            var topic = new DataChannel { Name = value.Name, Description = value.Description };
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
            var topic = new DataChannel { Id = id, Name = value.Name, Description = value.Description };
            return _controllerUtil.Put(topic);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _controllerUtil.Delete(id);
        }
    }
}
