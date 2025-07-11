using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.AlarmEvent;

public class  EventDetailEntity : NamedEntity<int>
{
}

/// <summary>
/// Change to EventInfo to avoid name conflict or confusion with the .net's Event class.
/// </summary>
[Table("event_record", Schema = SchemaNames.alarm_event)]
public class EventRecord : EventDetailEntity
{
}
