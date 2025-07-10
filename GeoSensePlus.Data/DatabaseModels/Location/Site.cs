using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
[Table("site", Schema = SchemaNames.location)]
public class Site : SiteEntity
{
    public List<Building> Buildings { get; set; }
}
