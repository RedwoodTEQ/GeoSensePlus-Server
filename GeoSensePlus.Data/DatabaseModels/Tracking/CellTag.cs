using System;

namespace GeoSensePlus.Data.DatabaseModels.Tracking;

public class CellTagEntity : NamedEntity<int>
{
    public DateTime TimeStamp { get; set; }
    public int TargetId { get; set; }   // foreign key to Target
}

/// <summary>
/// A cell tag is just a BLE beacon, which sends out BLE signals
/// </summary>
public class CellTag : CellTagEntity
{
    public Target Target { get; set; }
}
