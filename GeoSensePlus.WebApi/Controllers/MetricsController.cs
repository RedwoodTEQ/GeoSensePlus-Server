using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreUtils.Database.InfluxDb;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace GeoSensePlus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Produces("application/json")]
    public class MetricsController : Controller
    {
        IInfluxWriter _writer;
        IInfluxReader _reader;
        ILogger<MetricsController> _logger;

        public MetricsController(IInfluxWriter writer, IInfluxReader reader, ILogger<MetricsController> logger)
        {
            _writer = writer;
            _reader = reader;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PointModel<double> data)
        {

            if (data != null)
            {
                Console.WriteLine(JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
                await _writer.WriteAsync(data);
            }
            else
            {
                Console.WriteLine("data == null");
            }
            return Ok("ok");
        }

        [HttpGet("{metric}/{unit}/{range}")]
        public async Task<IActionResult> Get(string metric, string unit, int range)
        {
            _logger.LogInformation($"metric: {metric}; unit: {unit}; range: {range}");
            QueryRange queryRange;
            switch (unit)
            {
                case "hour":
                    queryRange = new QueryRange(range, RangeUnit.hour);
                    break;
                case "day":
                    queryRange = new QueryRange(range, RangeUnit.day);
                    break;
                default:
                    queryRange = new QueryRange(range, RangeUnit.minute);
                    break;
            }
            var data = await _reader.QueryAsync(metric, queryRange);
            if (data != null)
                return Json(data);
            else
                return NotFound();
        }
    }
}
