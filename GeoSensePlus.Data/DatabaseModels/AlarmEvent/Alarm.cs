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
    public string Severity { get; set; }
    public string Source { get; set; }
    /// <summary>
    /// Lifecycle state: acknowledge, restore, clear, etc
    /// </summary>
    public string State { get; set; }
}

public class Alarm : AlarmEntity
{
}
