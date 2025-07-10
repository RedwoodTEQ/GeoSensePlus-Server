using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Location;

public class SiteEntity : NamedEntity<int>
{
    // This class can be extended with additional properties if needed
}

/// <summary>
/// A site is a gsv service deployment
/// </summary>
public class Site : SiteEntity
{
    public List<Building> Buildings { get; set; }
}
