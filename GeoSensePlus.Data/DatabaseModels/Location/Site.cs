using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Location;

/// <summary>
/// A site is a gsv service deployment
/// </summary>
public class SiteEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
}
public class Site : SiteEntity, IIdAvailable<int>
{
    public int SiteId { get; set; }

    public List<Building> Buildings { get; set; }

    public int GetId()
    {
        return SiteId;
    }
}
