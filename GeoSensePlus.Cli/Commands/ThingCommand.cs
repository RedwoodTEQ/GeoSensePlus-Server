using GeoSensePlus.Cli.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Cli.Commands;

/// <summary>
/// For testing AWS IoT Core device shadow
/// </summary>
public class ThingCommand
{
    IThingService _svc;

    public ThingCommand(IThingService svc)
    {
        _svc = svc;
    }

    public async Task GetShadow()
    {
        await _svc.GetThingShadowAsync();
    }
}
