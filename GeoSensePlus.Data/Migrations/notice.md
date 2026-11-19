# Notice for GeoSensePlus.Data.Migrations

## NULLS NOT DISTINCT {#nulls_not_distinct-2509010319}

In order to ensure that NULL values are treated as distinct in PostgreSQL by
applying `NULLS NOT DISTINCT` on index creation so that the unique index for 2
("same_string", null) won't be allowed. The migration for model `PointGroup`
and `Point` on (Name, ParentId?) are added manually.

> NOTE: `NULLS NOT DISTINCT` is only available in PostgreSQL 15 and later.

The relevant configuration is in file `PointGroup.cs` ~ `class PointGroupConfiguration`:

```csharp
builder.HasIndex(p => new { p.Name, p.ParentId })
    .IsUnique().AreNullsDistinct(false)   // Apply "NULLs NOT DISTINCT" (for PostgreSQL v15+) - Two same names with null parent shouldn't be allowed
    .HasDatabaseName($"idx{nameof(PointGroup)}{nameof(PointGroup.Name)}{nameof(PointGroup.ParentId)}".ToSnakeCase());   // idx_point_group_name_parent_id
```

The original configuration is in th migration code, which has been deleted
after applying it with the above fluent API:

```csharp
public partial class _2 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            @"-- manually created in migration to apply 'NULLS NOT DISTINCT' option
              CREATE UNIQUE INDEX idx_point_group_name_parent_id
              ON messaging.point_group (name, parent_id)
              NULLS NOT DISTINCT;");

        migrationBuilder.Sql(
            @"-- manually created in migration to apply 'NULLS NOT DISTINCT' option
              CREATE UNIQUE INDEX idx_point_name_parent_id
              ON messaging.point (name, parent_id)
              NULLS NOT DISTINCT;");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            @"DROP INDEX IF EXISTS messaging.idx_point_group_name_parent_id;");

        migrationBuilder.Sql(
            @"DROP INDEX IF EXISTS messaging.idx_point_name_parent_id;");
    }
}
```


