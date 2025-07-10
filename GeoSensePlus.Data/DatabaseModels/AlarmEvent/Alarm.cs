using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels.AlarmEvent;


public class AlarmEntity : NamedEntity<int>
{
    /// <summary>
    /// Values:
    /// 4: fatal
    /// 3: failure or exception
    /// 2: warning
    /// 1: information with state change
    /// 0: information without state change
    /// </summary>

    [Column("severity")]
    public string Severity { get; set; }

    [Column("source")]
    public string Source { get; set; }

    /// <summary>
    /// Lifecycle state: acknowledge, restore, clear, etc
    /// </summary>
    [Column("state")]
    public string State { get; set; }
}

[Table("alarm", Schema = SchemaNames.alarm_event)]
public class Alarm : AlarmEntity
{
}
