using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GeoSensePlus.Data.Migrations;

/// <inheritdoc />
public partial class _20261118140501_3 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_point_group_point_group_parent_id",
            schema: "messaging",
            table: "point_group");

        migrationBuilder.DropTable(
            name: "topic",
            schema: "messaging");

        migrationBuilder.DropTable(
            name: "point",
            schema: "messaging");

        migrationBuilder.DropTable(
            name: "value",
            schema: "messaging");

        migrationBuilder.DropPrimaryKey(
            name: "PK_point_group",
            schema: "messaging",
            table: "point_group");

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "positioning",
            table: "uwb_tag",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "positioning",
            table: "uwb_tag",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "positioning",
            table: "uwb_anchor",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "positioning",
            table: "uwb_anchor",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "positioning",
            table: "uwb_anchor",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "positioning",
            table: "target",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "positioning",
            table: "target",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "positioning",
            table: "target",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "location",
            table: "site",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "location",
            table: "site",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "location",
            table: "site",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "sensing",
            table: "sensor",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "sensing",
            table: "sensor",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "messaging",
            table: "point_group",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "messaging",
            table: "point_group",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "sensing",
            table: "measure",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "sensing",
            table: "measure",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "sensing",
            table: "measure",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "map",
            table: "marker",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "map",
            table: "marker",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "map",
            table: "marker",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "positioning",
            table: "gps_tag",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "positioning",
            table: "gps_tag",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "map",
            table: "geofence",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "map",
            table: "geofence",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "map",
            table: "geofence",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "location",
            table: "floor_plan",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "location",
            table: "floor_plan",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "location",
            table: "floor_plan",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "location",
            table: "floor",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "location",
            table: "floor",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "location",
            table: "floor",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "alarm_event",
            table: "event_record",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "alarm_event",
            table: "event_record",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "alarm_event",
            table: "event_record",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "positioning",
            table: "cell_tag",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "positioning",
            table: "cell_tag",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "positioning",
            table: "cell_hub",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "positioning",
            table: "cell_hub",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "positioning",
            table: "cell_hub",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "positioning",
            table: "cell_anchor",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "positioning",
            table: "cell_anchor",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "positioning",
            table: "cell_anchor",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "location",
            table: "building",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "location",
            table: "building",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "location",
            table: "building",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "location",
            table: "area",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "location",
            table: "area",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "location",
            table: "area",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            schema: "alarm_event",
            table: "alarm",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            schema: "alarm_event",
            table: "alarm",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            schema: "alarm_event",
            table: "alarm",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddPrimaryKey(
            name: "pk_point_group",
            schema: "messaging",
            table: "point_group",
            column: "id");

        migrationBuilder.CreateTable(
            name: "data_point",
            schema: "messaging",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "text", nullable: true),
                description = table.Column<string>(type: "text", nullable: true),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                parent_id = table.Column<int>(type: "integer", nullable: false),
                scalar_value = table.Column<double>(type: "double precision", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_data_point", x => x.id);
                table.ForeignKey(
                    name: "fk__data_point__parent_id",
                    column: x => x.parent_id,
                    principalSchema: "messaging",
                    principalTable: "point_group",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "data_channel",
            schema: "messaging",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "text", nullable: true),
                description = table.Column<string>(type: "text", nullable: true),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                configuration = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                data_point_id = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_data_channel", x => x.id);
                table.ForeignKey(
                    name: "fk__data_channel__data_point_id",
                    column: x => x.data_point_id,
                    principalSchema: "messaging",
                    principalTable: "data_point",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "idx__point_group__name__parent_id",
            schema: "messaging",
            table: "point_group",
            columns: new[] { "name", "parent_id" },
            unique: true)
            .Annotation("Npgsql:NullsDistinct", false);

        migrationBuilder.CreateIndex(
            name: "IX_data_channel_data_point_id",
            schema: "messaging",
            table: "data_channel",
            column: "data_point_id");

        migrationBuilder.CreateIndex(
            name: "idx__data_point__name__parent_id",
            schema: "messaging",
            table: "data_point",
            columns: new[] { "name", "parent_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_data_point_parent_id",
            schema: "messaging",
            table: "data_point",
            column: "parent_id");

        migrationBuilder.AddForeignKey(
            name: "fk__point_group__parent_id",
            schema: "messaging",
            table: "point_group",
            column: "parent_id",
            principalSchema: "messaging",
            principalTable: "point_group",
            principalColumn: "id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk__point_group__parent_id",
            schema: "messaging",
            table: "point_group");

        migrationBuilder.DropTable(
            name: "data_channel",
            schema: "messaging");

        migrationBuilder.DropTable(
            name: "data_point",
            schema: "messaging");

        migrationBuilder.DropPrimaryKey(
            name: "pk_point_group",
            schema: "messaging",
            table: "point_group");

        migrationBuilder.DropIndex(
            name: "idx__point_group__name__parent_id",
            schema: "messaging",
            table: "point_group");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "positioning",
            table: "uwb_tag");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "positioning",
            table: "uwb_tag");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "positioning",
            table: "uwb_anchor");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "positioning",
            table: "uwb_anchor");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "positioning",
            table: "uwb_anchor");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "positioning",
            table: "target");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "positioning",
            table: "target");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "positioning",
            table: "target");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "location",
            table: "site");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "location",
            table: "site");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "location",
            table: "site");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "sensing",
            table: "sensor");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "sensing",
            table: "sensor");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "messaging",
            table: "point_group");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "messaging",
            table: "point_group");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "sensing",
            table: "measure");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "sensing",
            table: "measure");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "sensing",
            table: "measure");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "map",
            table: "marker");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "map",
            table: "marker");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "map",
            table: "marker");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "positioning",
            table: "gps_tag");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "positioning",
            table: "gps_tag");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "map",
            table: "geofence");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "map",
            table: "geofence");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "map",
            table: "geofence");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "location",
            table: "floor_plan");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "location",
            table: "floor_plan");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "location",
            table: "floor_plan");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "location",
            table: "floor");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "location",
            table: "floor");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "location",
            table: "floor");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "alarm_event",
            table: "event_record");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "alarm_event",
            table: "event_record");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "alarm_event",
            table: "event_record");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "positioning",
            table: "cell_tag");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "positioning",
            table: "cell_tag");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "positioning",
            table: "cell_hub");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "positioning",
            table: "cell_hub");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "positioning",
            table: "cell_hub");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "positioning",
            table: "cell_anchor");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "positioning",
            table: "cell_anchor");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "positioning",
            table: "cell_anchor");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "location",
            table: "building");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "location",
            table: "building");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "location",
            table: "building");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "location",
            table: "area");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "location",
            table: "area");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "location",
            table: "area");

        migrationBuilder.DropColumn(
            name: "created_at",
            schema: "alarm_event",
            table: "alarm");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            schema: "alarm_event",
            table: "alarm");

        migrationBuilder.DropColumn(
            name: "updated_at",
            schema: "alarm_event",
            table: "alarm");

        migrationBuilder.AddPrimaryKey(
            name: "PK_point_group",
            schema: "messaging",
            table: "point_group",
            column: "id");

        migrationBuilder.CreateTable(
            name: "value",
            schema: "messaging",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                boolean_value = table.Column<bool>(type: "boolean", nullable: true),
                description = table.Column<string>(type: "text", nullable: true),
                float_value = table.Column<float>(type: "real", nullable: true),
                integer_value = table.Column<int>(type: "integer", nullable: true),
                name = table.Column<string>(type: "text", nullable: true),
                type = table.Column<string>(type: "text", nullable: true),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                value_string = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_value", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "point",
            schema: "messaging",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                parent_id = table.Column<int>(type: "integer", nullable: false),
                value_id = table.Column<int>(type: "integer", nullable: true),
                description = table.Column<string>(type: "text", nullable: true),
                name = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_point", x => x.id);
                table.ForeignKey(
                    name: "FK_point_point_group_parent_id",
                    column: x => x.parent_id,
                    principalSchema: "messaging",
                    principalTable: "point_group",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_point_value_value_id",
                    column: x => x.value_id,
                    principalSchema: "messaging",
                    principalTable: "value",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "topic",
            schema: "messaging",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                description = table.Column<string>(type: "text", nullable: true),
                name = table.Column<string>(type: "text", nullable: true),
                PointId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_topic", x => x.id);
                table.ForeignKey(
                    name: "FK_topic_point_PointId",
                    column: x => x.PointId,
                    principalSchema: "messaging",
                    principalTable: "point",
                    principalColumn: "id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_point_parent_id",
            schema: "messaging",
            table: "point",
            column: "parent_id");

        migrationBuilder.CreateIndex(
            name: "IX_point_value_id",
            schema: "messaging",
            table: "point",
            column: "value_id");

        migrationBuilder.CreateIndex(
            name: "IX_topic_name",
            schema: "messaging",
            table: "topic",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_topic_PointId",
            schema: "messaging",
            table: "topic",
            column: "PointId");

        migrationBuilder.AddForeignKey(
            name: "FK_point_group_point_group_parent_id",
            schema: "messaging",
            table: "point_group",
            column: "parent_id",
            principalSchema: "messaging",
            principalTable: "point_group",
            principalColumn: "id",
            onDelete: ReferentialAction.Restrict);
    }
}
