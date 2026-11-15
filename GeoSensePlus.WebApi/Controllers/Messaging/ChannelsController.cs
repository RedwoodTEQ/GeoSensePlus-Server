using System.Linq;
using GeoSensePlus.Mongo.Models.Platform;
using GeoSensePlus.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreUtils.Database.MongoDb;

namespace GeoSensePlus.WebApi.Controllers.Messaging
{

    /// <summary>
    /// A channel is a message source (inbound) or target (outbound), e.g. a MQ server, blockchain, ROS, TTN etc.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ChannelsController(ILogger<ChannelsController> logger) : ControllerBase
    {
    }
}
