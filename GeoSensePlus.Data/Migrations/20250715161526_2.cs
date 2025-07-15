using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GeoSensePlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "positioning");

            migrationBuilder.EnsureSchema(
                name: "messaging");

            migrationBuilder.RenameTable(
                name: "uwb_tag",
                schema: "tracking",
                newName: "uwb_tag",
                newSchema: "positioning");

            migrationBuilder.RenameTable(
                name: "uwb_anchor",
                schema: "tracking",
                newName: "uwb_anchor",
                newSchema: "positioning");

            migrationBuilder.RenameTable(
                name: "topic",
                schema: "pub_sub",
                newName: "topic",
                newSchema: "messaging");

            migrationBuilder.RenameTable(
                name: "target",
                schema: "tracking",
                newName: "target",
                newSchema: "positioning");

            migrationBuilder.RenameTable(
                name: "gps_tag",
                schema: "tracking",
                newName: "gps_tag",
                newSchema: "positioning");

            migrationBuilder.RenameTable(
                name: "cell_tag",
                schema: "tracking",
                newName: "cell_tag",
                newSchema: "positioning");

            migrationBuilder.RenameTable(
                name: "cell_hub",
                schema: "tracking",
                newName: "cell_hub",
                newSchema: "positioning");

            migrationBuilder.RenameTable(
                name: "cell_anchor",
                schema: "tracking",
                newName: "cell_anchor",
                newSchema: "positioning");

            migrationBuilder.CreateTable(
                name: "point_group",
                schema: "messaging",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    parent_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_point_group", x => x.id);
                    table.ForeignKey(
                        name: "FK_point_group_point_group_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "messaging",
                        principalTable: "point_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "point",
                schema: "messaging",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    group_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_point", x => x.id);
                    table.ForeignKey(
                        name: "FK_point_point_group_group_id",
                        column: x => x.group_id,
                        principalSchema: "messaging",
                        principalTable: "point_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_point_group_id",
                schema: "messaging",
                table: "point",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_point_group_parent_id",
                schema: "messaging",
                table: "point_group",
                column: "parent_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "point",
                schema: "messaging");

            migrationBuilder.DropTable(
                name: "point_group",
                schema: "messaging");

            migrationBuilder.EnsureSchema(
                name: "tracking");

            migrationBuilder.EnsureSchema(
                name: "pub_sub");

            migrationBuilder.RenameTable(
                name: "uwb_tag",
                schema: "positioning",
                newName: "uwb_tag",
                newSchema: "tracking");

            migrationBuilder.RenameTable(
                name: "uwb_anchor",
                schema: "positioning",
                newName: "uwb_anchor",
                newSchema: "tracking");

            migrationBuilder.RenameTable(
                name: "topic",
                schema: "messaging",
                newName: "topic",
                newSchema: "pub_sub");

            migrationBuilder.RenameTable(
                name: "target",
                schema: "positioning",
                newName: "target",
                newSchema: "tracking");

            migrationBuilder.RenameTable(
                name: "gps_tag",
                schema: "positioning",
                newName: "gps_tag",
                newSchema: "tracking");

            migrationBuilder.RenameTable(
                name: "cell_tag",
                schema: "positioning",
                newName: "cell_tag",
                newSchema: "tracking");

            migrationBuilder.RenameTable(
                name: "cell_hub",
                schema: "positioning",
                newName: "cell_hub",
                newSchema: "tracking");

            migrationBuilder.RenameTable(
                name: "cell_anchor",
                schema: "positioning",
                newName: "cell_anchor",
                newSchema: "tracking");
        }
    }
}
