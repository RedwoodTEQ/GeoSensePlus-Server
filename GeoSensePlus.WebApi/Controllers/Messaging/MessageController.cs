﻿using GeoSensePlus.Core.MessageProcessing;
using GeoSensePlus.Mqtt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace GeoSensePlus.WebApi.Controllers.Messaging
{
    class RestChannelContext : ChannelContext<string>
    {
        private readonly HttpContext _httpContext;

        public RestChannelContext(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageEngine _messageEngine;
        private IMqttService _mqttSvc;

        public MessageController(IMessageEngine messageEngine, IMqttService mqttSvc)
        {
            _messageEngine = messageEngine;
            _mqttSvc = mqttSvc;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] object data)
        {
            if (data != null)
            {
                _messageEngine.Process(data.ToString(), new RestChannelContext(HttpContext));
                await _mqttSvc.PublishAsync("RawMessage", data.ToString());
            }
            return Ok("ok");
        }
    }
}
