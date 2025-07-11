using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GeoSensePlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "alarm_event");

            migrationBuilder.EnsureSchema(
                name: "location");

            migrationBuilder.EnsureSchema(
                name: "tracking");

            migrationBuilder.EnsureSchema(
                name: "map");

            migrationBuilder.EnsureSchema(
                name: "sensing");

            migrationBuilder.EnsureSchema(
                name: "pub_sub");

            migrationBuilder.CreateTable(
                name: "alarm",
                schema: "alarm_event",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    severity = table.Column<string>(type: "text", nullable: true),
                    source = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alarm", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cell_hub",
                schema: "tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cell_hub", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "event_record",
                schema: "alarm_event",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_record", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "geofence",
                schema: "map",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    shape = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geofence", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "measure",
                schema: "sensing",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    labels = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_measure", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "site",
                schema: "location",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "target",
                schema: "tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_target", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "topic",
                schema: "pub_sub",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topic", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "building",
                schema: "location",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    site_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_building", x => x.id);
                    table.ForeignKey(
                        name: "FK_building_site_site_id",
                        column: x => x.site_id,
                        principalSchema: "location",
                        principalTable: "site",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "gps_tag",
                schema: "tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    altitude = table.Column<double>(type: "double precision", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    target_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gps_tag", x => x.id);
                    table.ForeignKey(
                        name: "FK_gps_tag_target_target_id",
                        column: x => x.target_id,
                        principalSchema: "tracking",
                        principalTable: "target",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "uwb_tag",
                schema: "tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    axis_x = table.Column<double>(type: "double precision", nullable: false),
                    axis_y = table.Column<double>(type: "double precision", nullable: false),
                    axis_z = table.Column<double>(type: "double precision", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    target_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uwb_tag", x => x.id);
                    table.ForeignKey(
                        name: "FK_uwb_tag_target_target_id",
                        column: x => x.target_id,
                        principalSchema: "tracking",
                        principalTable: "target",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "floor",
                schema: "location",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BuildingId = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_floor", x => x.id);
                    table.ForeignKey(
                        name: "FK_floor_building_BuildingId",
                        column: x => x.BuildingId,
                        principalSchema: "location",
                        principalTable: "building",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "floor_plan",
                schema: "location",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BuildingId = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    file_location = table.Column<string>(type: "text", nullable: true),
                    floor_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_floor_plan", x => x.id);
                    table.ForeignKey(
                        name: "FK_floor_plan_building_BuildingId",
                        column: x => x.BuildingId,
                        principalSchema: "location",
                        principalTable: "building",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_floor_plan_floor_floor_id",
                        column: x => x.floor_id,
                        principalSchema: "location",
                        principalTable: "floor",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "area",
                schema: "location",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    floor_plan_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_area", x => x.id);
                    table.ForeignKey(
                        name: "FK_area_floor_plan_floor_plan_id",
                        column: x => x.floor_plan_id,
                        principalSchema: "location",
                        principalTable: "floor_plan",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "marker",
                schema: "map",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    axis_x = table.Column<double>(type: "double precision", nullable: false),
                    axis_y = table.Column<double>(type: "double precision", nullable: false),
                    is_poi = table.Column<bool>(type: "boolean", nullable: false),
                    floor_plan_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marker", x => x.id);
                    table.ForeignKey(
                        name: "FK_marker_floor_plan_floor_plan_id",
                        column: x => x.floor_plan_id,
                        principalSchema: "location",
                        principalTable: "floor_plan",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "uwb_anchor",
                schema: "tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    axis_x = table.Column<double>(type: "double precision", nullable: false),
                    axis_y = table.Column<double>(type: "double precision", nullable: false),
                    axis_z = table.Column<double>(type: "double precision", nullable: false),
                    config = table.Column<string>(type: "jsonb", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    floor_plan_id = table.Column<int>(type: "integer", nullable: true),
                    site_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uwb_anchor", x => x.id);
                    table.ForeignKey(
                        name: "FK_uwb_anchor_floor_plan_floor_plan_id",
                        column: x => x.floor_plan_id,
                        principalSchema: "location",
                        principalTable: "floor_plan",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_uwb_anchor_site_site_id",
                        column: x => x.site_id,
                        principalSchema: "location",
                        principalTable: "site",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "cell_anchor",
                schema: "tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    area_id = table.Column<int>(type: "integer", nullable: true),
                    cell_hub_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cell_anchor", x => x.id);
                    table.ForeignKey(
                        name: "FK_cell_anchor_area_area_id",
                        column: x => x.area_id,
                        principalSchema: "location",
                        principalTable: "area",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_cell_anchor_cell_hub_cell_hub_id",
                        column: x => x.cell_hub_id,
                        principalSchema: "tracking",
                        principalTable: "cell_hub",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "sensor",
                schema: "sensing",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    labels = table.Column<string>(type: "text", nullable: true),
                    value = table.Column<double>(type: "double precision", nullable: false),
                    unit = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    area_id = table.Column<int>(type: "integer", nullable: true),
                    measure_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sensor", x => x.id);
                    table.ForeignKey(
                        name: "FK_sensor_area_area_id",
                        column: x => x.area_id,
                        principalSchema: "location",
                        principalTable: "area",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_sensor_measure_measure_id",
                        column: x => x.measure_id,
                        principalSchema: "sensing",
                        principalTable: "measure",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "cell_tag",
                schema: "tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CellAnchorId = table.Column<int>(type: "integer", nullable: true),
                    CellHubId = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    target_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cell_tag", x => x.id);
                    table.ForeignKey(
                        name: "FK_cell_tag_cell_anchor_CellAnchorId",
                        column: x => x.CellAnchorId,
                        principalSchema: "tracking",
                        principalTable: "cell_anchor",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_cell_tag_cell_hub_CellHubId",
                        column: x => x.CellHubId,
                        principalSchema: "tracking",
                        principalTable: "cell_hub",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_cell_tag_target_target_id",
                        column: x => x.target_id,
                        principalSchema: "tracking",
                        principalTable: "target",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_area_floor_plan_id",
                schema: "location",
                table: "area",
                column: "floor_plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_building_site_id",
                schema: "location",
                table: "building",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "IX_cell_anchor_area_id",
                schema: "tracking",
                table: "cell_anchor",
                column: "area_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cell_anchor_cell_hub_id",
                schema: "tracking",
                table: "cell_anchor",
                column: "cell_hub_id");

            migrationBuilder.CreateIndex(
                name: "IX_cell_tag_CellAnchorId",
                schema: "tracking",
                table: "cell_tag",
                column: "CellAnchorId");

            migrationBuilder.CreateIndex(
                name: "IX_cell_tag_CellHubId",
                schema: "tracking",
                table: "cell_tag",
                column: "CellHubId");

            migrationBuilder.CreateIndex(
                name: "IX_cell_tag_target_id",
                schema: "tracking",
                table: "cell_tag",
                column: "target_id");

            migrationBuilder.CreateIndex(
                name: "IX_floor_BuildingId",
                schema: "location",
                table: "floor",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_floor_plan_BuildingId",
                schema: "location",
                table: "floor_plan",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_floor_plan_floor_id",
                schema: "location",
                table: "floor_plan",
                column: "floor_id");

            migrationBuilder.CreateIndex(
                name: "IX_gps_tag_target_id",
                schema: "tracking",
                table: "gps_tag",
                column: "target_id");

            migrationBuilder.CreateIndex(
                name: "IX_marker_floor_plan_id",
                schema: "map",
                table: "marker",
                column: "floor_plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_sensor_area_id",
                schema: "sensing",
                table: "sensor",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_sensor_measure_id",
                schema: "sensing",
                table: "sensor",
                column: "measure_id");

            migrationBuilder.CreateIndex(
                name: "IX_topic_name",
                schema: "pub_sub",
                table: "topic",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_uwb_anchor_floor_plan_id",
                schema: "tracking",
                table: "uwb_anchor",
                column: "floor_plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_uwb_anchor_site_id",
                schema: "tracking",
                table: "uwb_anchor",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "IX_uwb_tag_target_id",
                schema: "tracking",
                table: "uwb_tag",
                column: "target_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alarm",
                schema: "alarm_event");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "cell_tag",
                schema: "tracking");

            migrationBuilder.DropTable(
                name: "event_record",
                schema: "alarm_event");

            migrationBuilder.DropTable(
                name: "geofence",
                schema: "map");

            migrationBuilder.DropTable(
                name: "gps_tag",
                schema: "tracking");

            migrationBuilder.DropTable(
                name: "marker",
                schema: "map");

            migrationBuilder.DropTable(
                name: "sensor",
                schema: "sensing");

            migrationBuilder.DropTable(
                name: "topic",
                schema: "pub_sub");

            migrationBuilder.DropTable(
                name: "uwb_anchor",
                schema: "tracking");

            migrationBuilder.DropTable(
                name: "uwb_tag",
                schema: "tracking");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "cell_anchor",
                schema: "tracking");

            migrationBuilder.DropTable(
                name: "measure",
                schema: "sensing");

            migrationBuilder.DropTable(
                name: "target",
                schema: "tracking");

            migrationBuilder.DropTable(
                name: "area",
                schema: "location");

            migrationBuilder.DropTable(
                name: "cell_hub",
                schema: "tracking");

            migrationBuilder.DropTable(
                name: "floor_plan",
                schema: "location");

            migrationBuilder.DropTable(
                name: "floor",
                schema: "location");

            migrationBuilder.DropTable(
                name: "building",
                schema: "location");

            migrationBuilder.DropTable(
                name: "site",
                schema: "location");
        }
    }
}
