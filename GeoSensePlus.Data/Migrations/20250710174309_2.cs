using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoSensePlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_building_Sites_site_id",
                schema: "location",
                table: "building");

            migrationBuilder.DropForeignKey(
                name: "FK_UwbAnchors_Sites_site_id",
                table: "UwbAnchors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sites",
                table: "Sites");

            migrationBuilder.RenameTable(
                name: "Sites",
                newName: "site",
                newSchema: "location");

            migrationBuilder.AddPrimaryKey(
                name: "PK_site",
                schema: "location",
                table: "site",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_building_site_site_id",
                schema: "location",
                table: "building",
                column: "site_id",
                principalSchema: "location",
                principalTable: "site",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UwbAnchors_site_site_id",
                table: "UwbAnchors",
                column: "site_id",
                principalSchema: "location",
                principalTable: "site",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_building_site_site_id",
                schema: "location",
                table: "building");

            migrationBuilder.DropForeignKey(
                name: "FK_UwbAnchors_site_site_id",
                table: "UwbAnchors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_site",
                schema: "location",
                table: "site");

            migrationBuilder.RenameTable(
                name: "site",
                schema: "location",
                newName: "Sites");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sites",
                table: "Sites",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_building_Sites_site_id",
                schema: "location",
                table: "building",
                column: "site_id",
                principalTable: "Sites",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UwbAnchors_Sites_site_id",
                table: "UwbAnchors",
                column: "site_id",
                principalTable: "Sites",
                principalColumn: "id");
        }
    }
}
