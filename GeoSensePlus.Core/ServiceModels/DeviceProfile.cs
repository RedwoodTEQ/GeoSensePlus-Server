using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Core.ServiceModels;

public class DeviceProfile
{
    public string DeviceName { get; set; }
    public string DeviceType { get; set; } = Consts.unknown;

    /// <summary>
    /// Json format device configuration
    /// </summary>
    public string DeviceConfig { get; set; }
}
