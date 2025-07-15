using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoSensePlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_point_point_group_group_id",
                schema: "messaging",
                table: "point");

            migrationBuilder.RenameColumn(
                name: "group_id",
                schema: "messaging",
                table: "point",
                newName: "parent_id");

            migrationBuilder.RenameIndex(
                name: "IX_point_group_id",
                schema: "messaging",
                table: "point",
                newName: "IX_point_parent_id");

            migrationBuilder.AddForeignKey(
                name: "FK_point_point_group_parent_id",
                schema: "messaging",
                table: "point",
                column: "parent_id",
                principalSchema: "messaging",
                principalTable: "point_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_point_point_group_parent_id",
                schema: "messaging",
                table: "point");

            migrationBuilder.RenameColumn(
                name: "parent_id",
                schema: "messaging",
                table: "point",
                newName: "group_id");

            migrationBuilder.RenameIndex(
                name: "IX_point_parent_id",
                schema: "messaging",
                table: "point",
                newName: "IX_point_group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_point_point_group_group_id",
                schema: "messaging",
                table: "point",
                column: "group_id",
                principalSchema: "messaging",
                principalTable: "point_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
