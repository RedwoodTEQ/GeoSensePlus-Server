using GeoSensePlus.Core.MessageProcessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeoSensePlus.Server.Controllers
{
    class RestChannelContext : ChannelContext<string>
    {
        private readonly HttpContext _httpContext;

        public RestChannelContext(HttpContext httpContext)
        {
            this._httpContext = httpContext;
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class TextDataController : Controller
    {
        private readonly IMessageEngine _messageEngine;

        public TextDataController(IMessageEngine messageEngine)
        {
            _messageEngine = messageEngine;
        }

        [HttpPost]
        public IActionResult Post([FromBody]object data)
        {
            if(data != null)
                _messageEngine.Process(data.ToString(), new RestChannelContext(HttpContext));
            return Ok("ok");
        }
    }
}
