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
                name: "FK_CellAnchors_CellHubs_cell_hub_id",
                table: "CellAnchors");

            migrationBuilder.DropForeignKey(
                name: "FK_CellAnchors_area_area_id",
                table: "CellAnchors");

            migrationBuilder.DropForeignKey(
                name: "FK_CellTags_CellAnchors_CellAnchorId",
                table: "CellTags");

            migrationBuilder.DropForeignKey(
                name: "FK_CellTags_CellHubs_CellHubId",
                table: "CellTags");

            migrationBuilder.DropForeignKey(
                name: "FK_CellTags_Targets_target_id",
                table: "CellTags");

            migrationBuilder.DropForeignKey(
                name: "FK_GpsTags_Targets_target_id",
                table: "GpsTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Measures_measure_id",
                table: "Sensors");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_area_area_id",
                table: "Sensors");

            migrationBuilder.DropForeignKey(
                name: "FK_UwbAnchors_floor_plan_floor_plan_id",
                table: "UwbAnchors");

            migrationBuilder.DropForeignKey(
                name: "FK_UwbAnchors_site_site_id",
                table: "UwbAnchors");

            migrationBuilder.DropForeignKey(
                name: "FK_UwbTags_Targets_target_id",
                table: "UwbTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UwbTags",
                table: "UwbTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UwbAnchors",
                table: "UwbAnchors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Targets",
                table: "Targets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sensors",
                table: "Sensors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Measures",
                table: "Measures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GpsTags",
                table: "GpsTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CellTags",
                table: "CellTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CellHubs",
                table: "CellHubs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CellAnchors",
                table: "CellAnchors");

            migrationBuilder.EnsureSchema(
                name: "tracking");

            migrationBuilder.EnsureSchema(
                name: "sensing");

            migrationBuilder.EnsureSchema(
                name: "pub_sub");

            migrationBuilder.RenameTable(
                name: "topic",
                schema: "map",
                newName: "topic",
                newSchema: "pub_sub");

            migrationBuilder.RenameTable(
                name: "UwbTags",
                newName: "uwb_tag",
                newSchema: "tracking");

            migrationBuilder.RenameTable(
                name: "UwbAnchors",
                newName: "uwb_anchor",
                newSchema: "tracking");

            migrationBuilder.RenameTable(
                name: "Targets",
                newName: "target",
                newSchema: "tracking");

            migrationBuilder.RenameTable(
                name: "Sensors",
                newName: "sensor",
                newSchema: "sensing");

            migrationBuilder.RenameTable(
                name: "Measures",
                newName: "measure",
                newSchema: "sensing");

            migrationBuilder.RenameTable(
                name: "GpsTags",
                newName: "gps_tag",
                newSchema: "tracking");

            migrationBuilder.RenameTable(
                name: "CellTags",
                newName: "cell_tag",
                newSchema: "tracking");

            migrationBuilder.RenameTable(
                name: "CellHubs",
                newName: "cell_hub",
                newSchema: "tracking");

            migrationBuilder.RenameTable(
                name: "CellAnchors",
                newName: "cell_anchor",
                newSchema: "tracking");

            migrationBuilder.RenameIndex(
                name: "IX_UwbTags_target_id",
                schema: "tracking",
                table: "uwb_tag",
                newName: "IX_uwb_tag_target_id");

            migrationBuilder.RenameIndex(
                name: "IX_UwbAnchors_site_id",
                schema: "tracking",
                table: "uwb_anchor",
                newName: "IX_uwb_anchor_site_id");

            migrationBuilder.RenameIndex(
                name: "IX_UwbAnchors_floor_plan_id",
                schema: "tracking",
                table: "uwb_anchor",
                newName: "IX_uwb_anchor_floor_plan_id");

            migrationBuilder.RenameIndex(
                name: "IX_Sensors_measure_id",
                schema: "sensing",
                table: "sensor",
                newName: "IX_sensor_measure_id");

            migrationBuilder.RenameIndex(
                name: "IX_Sensors_area_id",
                schema: "sensing",
                table: "sensor",
                newName: "IX_sensor_area_id");

            migrationBuilder.RenameIndex(
                name: "IX_GpsTags_target_id",
                schema: "tracking",
                table: "gps_tag",
                newName: "IX_gps_tag_target_id");

            migrationBuilder.RenameIndex(
                name: "IX_CellTags_target_id",
                schema: "tracking",
                table: "cell_tag",
                newName: "IX_cell_tag_target_id");

            migrationBuilder.RenameIndex(
                name: "IX_CellTags_CellHubId",
                schema: "tracking",
                table: "cell_tag",
                newName: "IX_cell_tag_CellHubId");

            migrationBuilder.RenameIndex(
                name: "IX_CellTags_CellAnchorId",
                schema: "tracking",
                table: "cell_tag",
                newName: "IX_cell_tag_CellAnchorId");

            migrationBuilder.RenameIndex(
                name: "IX_CellAnchors_cell_hub_id",
                schema: "tracking",
                table: "cell_anchor",
                newName: "IX_cell_anchor_cell_hub_id");

            migrationBuilder.RenameIndex(
                name: "IX_CellAnchors_area_id",
                schema: "tracking",
                table: "cell_anchor",
                newName: "IX_cell_anchor_area_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_uwb_tag",
                schema: "tracking",
                table: "uwb_tag",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_uwb_anchor",
                schema: "tracking",
                table: "uwb_anchor",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_target",
                schema: "tracking",
                table: "target",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sensor",
                schema: "sensing",
                table: "sensor",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_measure",
                schema: "sensing",
                table: "measure",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_gps_tag",
                schema: "tracking",
                table: "gps_tag",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cell_tag",
                schema: "tracking",
                table: "cell_tag",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cell_hub",
                schema: "tracking",
                table: "cell_hub",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cell_anchor",
                schema: "tracking",
                table: "cell_anchor",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_cell_anchor_area_area_id",
                schema: "tracking",
                table: "cell_anchor",
                column: "area_id",
                principalSchema: "location",
                principalTable: "area",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_cell_anchor_cell_hub_cell_hub_id",
                schema: "tracking",
                table: "cell_anchor",
                column: "cell_hub_id",
                principalSchema: "tracking",
                principalTable: "cell_hub",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_cell_tag_cell_anchor_CellAnchorId",
                schema: "tracking",
                table: "cell_tag",
                column: "CellAnchorId",
                principalSchema: "tracking",
                principalTable: "cell_anchor",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_cell_tag_cell_hub_CellHubId",
                schema: "tracking",
                table: "cell_tag",
                column: "CellHubId",
                principalSchema: "tracking",
                principalTable: "cell_hub",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_cell_tag_target_target_id",
                schema: "tracking",
                table: "cell_tag",
                column: "target_id",
                principalSchema: "tracking",
                principalTable: "target",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_gps_tag_target_target_id",
                schema: "tracking",
                table: "gps_tag",
                column: "target_id",
                principalSchema: "tracking",
                principalTable: "target",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_sensor_area_area_id",
                schema: "sensing",
                table: "sensor",
                column: "area_id",
                principalSchema: "location",
                principalTable: "area",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_sensor_measure_measure_id",
                schema: "sensing",
                table: "sensor",
                column: "measure_id",
                principalSchema: "sensing",
                principalTable: "measure",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_uwb_anchor_floor_plan_floor_plan_id",
                schema: "tracking",
                table: "uwb_anchor",
                column: "floor_plan_id",
                principalSchema: "location",
                principalTable: "floor_plan",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_uwb_anchor_site_site_id",
                schema: "tracking",
                table: "uwb_anchor",
                column: "site_id",
                principalSchema: "location",
                principalTable: "site",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_uwb_tag_target_target_id",
                schema: "tracking",
                table: "uwb_tag",
                column: "target_id",
                principalSchema: "tracking",
                principalTable: "target",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cell_anchor_area_area_id",
                schema: "tracking",
                table: "cell_anchor");

            migrationBuilder.DropForeignKey(
                name: "FK_cell_anchor_cell_hub_cell_hub_id",
                schema: "tracking",
                table: "cell_anchor");

            migrationBuilder.DropForeignKey(
                name: "FK_cell_tag_cell_anchor_CellAnchorId",
                schema: "tracking",
                table: "cell_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_cell_tag_cell_hub_CellHubId",
                schema: "tracking",
                table: "cell_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_cell_tag_target_target_id",
                schema: "tracking",
                table: "cell_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_gps_tag_target_target_id",
                schema: "tracking",
                table: "gps_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_sensor_area_area_id",
                schema: "sensing",
                table: "sensor");

            migrationBuilder.DropForeignKey(
                name: "FK_sensor_measure_measure_id",
                schema: "sensing",
                table: "sensor");

            migrationBuilder.DropForeignKey(
                name: "FK_uwb_anchor_floor_plan_floor_plan_id",
                schema: "tracking",
                table: "uwb_anchor");

            migrationBuilder.DropForeignKey(
                name: "FK_uwb_anchor_site_site_id",
                schema: "tracking",
                table: "uwb_anchor");

            migrationBuilder.DropForeignKey(
                name: "FK_uwb_tag_target_target_id",
                schema: "tracking",
                table: "uwb_tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_uwb_tag",
                schema: "tracking",
                table: "uwb_tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_uwb_anchor",
                schema: "tracking",
                table: "uwb_anchor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_target",
                schema: "tracking",
                table: "target");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sensor",
                schema: "sensing",
                table: "sensor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_measure",
                schema: "sensing",
                table: "measure");

            migrationBuilder.DropPrimaryKey(
                name: "PK_gps_tag",
                schema: "tracking",
                table: "gps_tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cell_tag",
                schema: "tracking",
                table: "cell_tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cell_hub",
                schema: "tracking",
                table: "cell_hub");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cell_anchor",
                schema: "tracking",
                table: "cell_anchor");

            migrationBuilder.RenameTable(
                name: "topic",
                schema: "pub_sub",
                newName: "topic",
                newSchema: "map");

            migrationBuilder.RenameTable(
                name: "uwb_tag",
                schema: "tracking",
                newName: "UwbTags");

            migrationBuilder.RenameTable(
                name: "uwb_anchor",
                schema: "tracking",
                newName: "UwbAnchors");

            migrationBuilder.RenameTable(
                name: "target",
                schema: "tracking",
                newName: "Targets");

            migrationBuilder.RenameTable(
                name: "sensor",
                schema: "sensing",
                newName: "Sensors");

            migrationBuilder.RenameTable(
                name: "measure",
                schema: "sensing",
                newName: "Measures");

            migrationBuilder.RenameTable(
                name: "gps_tag",
                schema: "tracking",
                newName: "GpsTags");

            migrationBuilder.RenameTable(
                name: "cell_tag",
                schema: "tracking",
                newName: "CellTags");

            migrationBuilder.RenameTable(
                name: "cell_hub",
                schema: "tracking",
                newName: "CellHubs");

            migrationBuilder.RenameTable(
                name: "cell_anchor",
                schema: "tracking",
                newName: "CellAnchors");

            migrationBuilder.RenameIndex(
                name: "IX_uwb_tag_target_id",
                table: "UwbTags",
                newName: "IX_UwbTags_target_id");

            migrationBuilder.RenameIndex(
                name: "IX_uwb_anchor_site_id",
                table: "UwbAnchors",
                newName: "IX_UwbAnchors_site_id");

            migrationBuilder.RenameIndex(
                name: "IX_uwb_anchor_floor_plan_id",
                table: "UwbAnchors",
                newName: "IX_UwbAnchors_floor_plan_id");

            migrationBuilder.RenameIndex(
                name: "IX_sensor_measure_id",
                table: "Sensors",
                newName: "IX_Sensors_measure_id");

            migrationBuilder.RenameIndex(
                name: "IX_sensor_area_id",
                table: "Sensors",
                newName: "IX_Sensors_area_id");

            migrationBuilder.RenameIndex(
                name: "IX_gps_tag_target_id",
                table: "GpsTags",
                newName: "IX_GpsTags_target_id");

            migrationBuilder.RenameIndex(
                name: "IX_cell_tag_target_id",
                table: "CellTags",
                newName: "IX_CellTags_target_id");

            migrationBuilder.RenameIndex(
                name: "IX_cell_tag_CellHubId",
                table: "CellTags",
                newName: "IX_CellTags_CellHubId");

            migrationBuilder.RenameIndex(
                name: "IX_cell_tag_CellAnchorId",
                table: "CellTags",
                newName: "IX_CellTags_CellAnchorId");

            migrationBuilder.RenameIndex(
                name: "IX_cell_anchor_cell_hub_id",
                table: "CellAnchors",
                newName: "IX_CellAnchors_cell_hub_id");

            migrationBuilder.RenameIndex(
                name: "IX_cell_anchor_area_id",
                table: "CellAnchors",
                newName: "IX_CellAnchors_area_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UwbTags",
                table: "UwbTags",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UwbAnchors",
                table: "UwbAnchors",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Targets",
                table: "Targets",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sensors",
                table: "Sensors",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Measures",
                table: "Measures",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GpsTags",
                table: "GpsTags",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CellTags",
                table: "CellTags",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CellHubs",
                table: "CellHubs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CellAnchors",
                table: "CellAnchors",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CellAnchors_CellHubs_cell_hub_id",
                table: "CellAnchors",
                column: "cell_hub_id",
                principalTable: "CellHubs",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CellAnchors_area_area_id",
                table: "CellAnchors",
                column: "area_id",
                principalSchema: "location",
                principalTable: "area",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CellTags_CellAnchors_CellAnchorId",
                table: "CellTags",
                column: "CellAnchorId",
                principalTable: "CellAnchors",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CellTags_CellHubs_CellHubId",
                table: "CellTags",
                column: "CellHubId",
                principalTable: "CellHubs",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CellTags_Targets_target_id",
                table: "CellTags",
                column: "target_id",
                principalTable: "Targets",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_GpsTags_Targets_target_id",
                table: "GpsTags",
                column: "target_id",
                principalTable: "Targets",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Measures_measure_id",
                table: "Sensors",
                column: "measure_id",
                principalTable: "Measures",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_area_area_id",
                table: "Sensors",
                column: "area_id",
                principalSchema: "location",
                principalTable: "area",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UwbAnchors_floor_plan_floor_plan_id",
                table: "UwbAnchors",
                column: "floor_plan_id",
                principalSchema: "location",
                principalTable: "floor_plan",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UwbAnchors_site_site_id",
                table: "UwbAnchors",
                column: "site_id",
                principalSchema: "location",
                principalTable: "site",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UwbTags_Targets_target_id",
                table: "UwbTags",
                column: "target_id",
                principalTable: "Targets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
