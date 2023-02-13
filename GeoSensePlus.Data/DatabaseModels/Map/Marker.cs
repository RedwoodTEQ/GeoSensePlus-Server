using GeoSensePlus.Data.DatabaseModels.Base;
using GeoSensePlus.Data.DatabaseModels.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Map;

/// <summary>
/// Indoor use only, if need an outdoor POI, use Geofence instead
/// </summary>
public class MarkerEntity
{
    public string Name { get; set; }
    public double AxisX { get; set; } = -1;
    public double AxisY { get; set; } = -1;
    public bool IsPoi { get; set; } = false;

}

/// <summary>
/// Renamed from "Poi"
/// </summary>
public class Marker : MarkerEntity, IIdAvailable<int>
{
    public int PoiId { get; set; }

    public Floorplan Floorplan { get; set; }

    public int GetId()
    {
        return PoiId;
    }
}
