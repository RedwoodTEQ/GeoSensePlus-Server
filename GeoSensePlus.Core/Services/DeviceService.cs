using GeoSensePlus.Core.ServiceModels;
using GeoSensePlus.Data;
using GeoSensePlus.Data.DatabaseModels.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Core.Services;

/// <summary>
/// Responsible for hardware/device management
/// </summary>
public interface IDeviceService
{
    object AddDevice(UserInfo userInfo, DeviceProfile profile);
    public void UpdateFirmware(string deviceId);
    void LinkDeviceWithMarker(int anchorId, int markerId);
}

public class DeviceService : IDeviceService
{
    ApplicationDbContext _ctx { get; set; }

    public DeviceService(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public object AddDevice(UserInfo userInfo, DeviceProfile profile)
    {
        switch (profile.DeviceType)
        {
            case Consts.uwb_anchor:
                return UwbAnchor.Create(_ctx, profile.DeviceName, profile.DeviceConfig);
        }

        return null;
    }

    public void UpdateFirmware(string deviceId)
    {
        throw new NotImplementedException();
    }

    public void LinkDeviceWithMarker(int anchorId, int markerId)    // TODO: current - LinkDeviceWithMarker
    {
        throw new NotImplementedException();
    }
}
