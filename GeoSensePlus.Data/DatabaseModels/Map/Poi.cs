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
public class PoiEntity
{
    public string Name { get; set; }
    public double AxisX { get; set; } = -1;
    public double AxisY { get; set; } = -1;

}
public class Poi : PoiEntity, IIdAvailable<int>
{
    public int PoiId { get; set; }

    public Floorplan Floorplan { get; set; }

    public int GetId()
    {
        return PoiId;
    }
}
