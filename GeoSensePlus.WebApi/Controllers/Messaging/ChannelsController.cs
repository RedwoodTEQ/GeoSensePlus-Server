using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GeoSensePlus.WebApi.Controllers.Messaging;

/// <summary>
/// <see cref="GeoSensePlus.Data.DatabaseModels.Messaging.Topic"/>
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ChannelsController(ILogger<ChannelsController> logger) : ControllerBase
{
}
