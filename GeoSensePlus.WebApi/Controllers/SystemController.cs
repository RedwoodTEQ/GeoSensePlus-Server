using GeoSensePlus.Firestore.ConfigUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemController: ControllerBase
    {
        IConfigOperator _configOperator;

        public SystemController(IConfigOperator configOperator)
        {
            _configOperator = configOperator;
        }

        [HttpGet("firebase-key")]
        public string FirebaseKey()
        {
            return _configOperator.GetFirebaseKey();
        }

        [HttpGet("version")]
        public string Version()
        {
            return new SystemInfo().GetInfo();
        }
    }
}
