using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GeoSensePlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class _4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PointId",
                schema: "messaging",
                table: "topic",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "value_id",
                schema: "messaging",
                table: "point",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "value",
                schema: "messaging",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    value_string = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    integer_value = table.Column<int>(type: "integer", nullable: true),
                    float_value = table.Column<float>(type: "real", nullable: true),
                    boolean_value = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_value", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_topic_PointId",
                schema: "messaging",
                table: "topic",
                column: "PointId");

            migrationBuilder.CreateIndex(
                name: "IX_point_value_id",
                schema: "messaging",
                table: "point",
                column: "value_id");

            migrationBuilder.AddForeignKey(
                name: "FK_point_value_value_id",
                schema: "messaging",
                table: "point",
                column: "value_id",
                principalSchema: "messaging",
                principalTable: "value",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_topic_point_PointId",
                schema: "messaging",
                table: "topic",
                column: "PointId",
                principalSchema: "messaging",
                principalTable: "point",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_point_value_value_id",
                schema: "messaging",
                table: "point");

            migrationBuilder.DropForeignKey(
                name: "FK_topic_point_PointId",
                schema: "messaging",
                table: "topic");

            migrationBuilder.DropTable(
                name: "value",
                schema: "messaging");

            migrationBuilder.DropIndex(
                name: "IX_topic_PointId",
                schema: "messaging",
                table: "topic");

            migrationBuilder.DropIndex(
                name: "IX_point_value_id",
                schema: "messaging",
                table: "point");

            migrationBuilder.DropColumn(
                name: "PointId",
                schema: "messaging",
                table: "topic");

            migrationBuilder.DropColumn(
                name: "value_id",
                schema: "messaging",
                table: "point");
        }
    }
}
