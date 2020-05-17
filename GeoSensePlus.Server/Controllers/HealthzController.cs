using GeoSensePlus.Firestore.ConfigUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthzController: Controller
    {
        IConfigOperator _configOperator;

        public HealthzController(IConfigOperator configOperator)
        {
            _configOperator = configOperator;
        }

        [HttpGet]
        public string Get()
        {
            return $@"
Healthz responds correctly

Current key file: {_configOperator.GetFirebaseKey()}
";
        }
    }
}
