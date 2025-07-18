# Notice for GeoSensePlus.Data.Migrations

## NULLS NOT DISTINCT 

In order to ensure that NULL values are treated as distinct in PostgreSQL by
applying `NULLS NOT DISTINCT` on index creation so that  the unique index for 2
("same_string", null) won't be allowed. The migration for model `PointGroup`
and `Point` on (Name, ParentId?) are added manually.

> NOTE: `NULLS NOT DISTINCT` is only available in PostgreSQL 15 and later.

This is the migration code from file `20250718132222_5.cs`:

```csharp
public partial class _5 : Migration
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