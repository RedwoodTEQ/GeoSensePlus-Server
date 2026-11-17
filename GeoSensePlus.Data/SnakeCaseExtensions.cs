using System.Linq;

namespace GeoSensePlus.Data;

/// <summary>
/// Snake-case conversion for PostgreSQL naming conventions, e.g. "PointGroup" → "point_group".
/// Used for constraint/index names in <see cref="DatabaseModels.BaseEntityConfiguration{TEntity,TKey}"/>.
/// NOTE: consecutive uppercase letters are split ("UWBAnchor" → "u_w_b_anchor"), so avoid acronyms in entity names.
/// </summary>
public static class SnakeCaseExtensions
{
    public static string ToSnakeCase(this string s) =>
        string.Concat(s.Select((c, i) =>
            i > 0 && char.IsUpper(c) ? "_" + char.ToLowerInvariant(c) : char.ToLowerInvariant(c).ToString()));
}
