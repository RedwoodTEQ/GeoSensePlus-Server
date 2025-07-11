using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.Tracking;

public class TargetEntity : NamedEntity<int>
{
    // This class is used for API requests, so it does not need any additional properties.
}

/// <summary>
/// A target is a tracking target, which can be an asset or a person.
/// </summary>
[Table("target", Schema = SchemaNames.tracking)]
public class Target : TargetEntity
{
    //public Area CacheArea { get; set; }

    public List<CellTag> CellTags { get; set; } = new List<CellTag>();
    public List<UwbTag> UwbTags { get; set; } = new List<UwbTag>();
    public List<GpsTag> GpsTags { get; set; } = new List<GpsTag>();
}
