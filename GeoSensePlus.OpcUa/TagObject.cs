namespace GeoSensePlus.OpcUa;
public class TagObject
{
    public TagObject(string displayName, string identifier)
    {
        DisplayName = displayName;
        Identifier = identifier;
    }

    public DateTime LastUpdatedTime { get; set; }
    public DateTime LastSourceTimeStamp { get; set; }
    public string? StatusCode { get; set; }
    public string? LastGoodValue { get; set; }
    public string? CurrentValue { get; set; }
    public string Identifier { get; set; }
    public string DisplayName { get; set; }
}
