using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Messaging;

public class ValueEntity : BaseEntity<int>
{
    [Column("type")]
    public string Type { get; set; } = string.Empty;  // e.g. "string", "int", "float", "bool", etc.

    [Column("value_string")]
    public string ValueString { get; set; } = string.Empty;  // serialized value, e.g. "123", "true", "hello world"

    [Column("integer_value")]
    public int? IntegerValue { get; set; }

    [Column("float_value")]
    public float? FloatValue { get; set; }

    [Column("boolean_value")]
    public bool? BooleanValue { get; set; }
}

// _todo: Merge this class into DataPoint class, and remove the Value class.
[Obsolete("This class is deprecated and will be merged into the DataPoint class.")]
[Table("value", Schema = SchemaNames.messaging)]
public class Value : ValueEntity
{
    [JsonIgnore]    // ignore in web response serialization, to avoid circular references
    public List<DataPoint> Points { get; set; } = new List<DataPoint>();  // data points that this value is associated with
}
