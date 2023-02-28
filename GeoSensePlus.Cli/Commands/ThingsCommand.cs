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
public class ThingsCommand
{
    IThingService _svc;

    public ThingsCommand(IThingService svc)
    {
        _svc = svc;
    }

    public async Task Shadow()
    {
        await _svc.GetThingShadowAsync();
    }

    public async Task All()
    {
        await _svc.ListThingsAsync();
    }

    public async Task MqttPub()
    {
        await _svc.MqttPubAsync();
    }

    public async Task Cert()
    {
        await _svc.CreateKeysAndCertificateAsync();
    }
}
