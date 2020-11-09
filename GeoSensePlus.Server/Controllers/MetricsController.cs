using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreUtils.Database.InfluxDb;
using System.Text.Json;

namespace GeoSensePlus.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricsController : Controller
    {
        IInfluxWriter _writer;

        public MetricsController(IInfluxWriter writer)
        {
            _writer = writer;
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
    }
}
