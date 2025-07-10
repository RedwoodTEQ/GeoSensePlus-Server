namespace GeoSensePlus.Data.DatabaseModels.AlarmEvent;

public class  EventDetailEntity : NamedEntity<int>
{
}

/// <summary>
/// Change to EventInfo to avoid name conflict or confusion with the .net's Event class.
/// </summary>
public class EventDetail : EventDetailEntity
{
}
