using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Messaging;

public class ValueEntity
{
    [Column("type")]
    public string Type { get; set; } = string.Empty;  // e.g. "string", "int", "float", "bool", etc.

    [Column("value_string")]
    public string ValueString { get; set; } = string.Empty;  // serialized value, e.g. "123", "true", "hello world"

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("integer_value")]
    public int? IntegerValue { get; set; }

    [Column("float_value")]
    public float? FloatValue { get; set; }

    [Column("boolean_value")]
    public bool? BooleanValue { get; set; }
}

[Table("value", Schema = SchemaNames.messaging)]
public class Value : NamedEntity<int>
{
    public List<Point> Points { get; set; } = new List<Point>();  // points that this value is associated with
}
