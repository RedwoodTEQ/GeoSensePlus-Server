using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoSensePlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class _2_manul : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /**
             * This migration is created to apply the 'NULLS NOT DISTINCT'
             * option.
             * 
             * By manully creating the unique indexes with this option, it's
             * not needed to use [index] in the model classes.
             */

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
}
