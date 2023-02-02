﻿// <auto-generated />
using System;
using GeoSensePlus.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GeoSensePlus.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.AlarmEvent", b =>
                {
                    b.Property<int>("AlarmEventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AlarmEventId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Severity")
                        .HasColumnType("text");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .HasColumnType("text");

                    b.HasKey("AlarmEventId");

                    b.ToTable("AlarmEvents");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Area", b =>
                {
                    b.Property<int>("AreaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AreaId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("EdgeId")
                        .HasColumnType("integer");

                    b.Property<int?>("FloorplanId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("AreaId");

                    b.HasIndex("EdgeId")
                        .IsUnique();

                    b.HasIndex("FloorplanId");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Building", b =>
                {
                    b.Property<int>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("BuildingId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("BuildingId");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.CellTag", b =>
                {
                    b.Property<int>("CellTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CellTagId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("EdgeId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("TargetId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("CellTagId");

                    b.HasIndex("EdgeId");

                    b.HasIndex("TargetId");

                    b.ToTable("CellTags");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Edge", b =>
                {
                    b.Property<int>("EdgeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EdgeId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("GatewayId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("EdgeId");

                    b.HasIndex("GatewayId");

                    b.ToTable("Edges");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Floorplan", b =>
                {
                    b.Property<int>("FloorplanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FloorplanId"));

                    b.Property<int?>("BuildingId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("FileLocation")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("FloorplanId");

                    b.HasIndex("BuildingId");

                    b.ToTable("Floorplans");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Gateway", b =>
                {
                    b.Property<int>("GatewayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GatewayId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("GatewayId");

                    b.ToTable("Gateway");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Geofence", b =>
                {
                    b.Property<int>("GeofenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GeofenceId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("GeofenceId");

                    b.ToTable("Geofences");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.GpsTag", b =>
                {
                    b.Property<int>("GpsTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GpsTagId"));

                    b.Property<double>("Altitude")
                        .HasColumnType("double precision");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("TargetId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("GpsTagId");

                    b.HasIndex("TargetId");

                    b.ToTable("GpsTags");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Measure", b =>
                {
                    b.Property<int>("MeasureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MeasureId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Labels")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("MeasureId");

                    b.ToTable("Measures");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Sensor", b =>
                {
                    b.Property<int>("SensorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SensorId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("EdgeId")
                        .HasColumnType("integer");

                    b.Property<string>("Labels")
                        .HasColumnType("text");

                    b.Property<int?>("MeasureId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<string>("Unit")
                        .HasColumnType("text");

                    b.Property<double>("Value")
                        .HasColumnType("double precision");

                    b.HasKey("SensorId");

                    b.HasIndex("EdgeId");

                    b.HasIndex("MeasureId");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Target", b =>
                {
                    b.Property<int>("TargetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TargetId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("TargetId");

                    b.ToTable("Targets");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.UwbTag", b =>
                {
                    b.Property<int>("UwbTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UwbTagId"));

                    b.Property<double>("AxisX")
                        .HasColumnType("double precision");

                    b.Property<double>("AxisY")
                        .HasColumnType("double precision");

                    b.Property<double>("AxisZ")
                        .HasColumnType("double precision");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("TargetId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("UwbTagId");

                    b.HasIndex("TargetId");

                    b.ToTable("UwbTags");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Area", b =>
                {
                    b.HasOne("GeoSensePlus.Data.DatabaseModels.Edge", "Edge")
                        .WithOne("Area")
                        .HasForeignKey("GeoSensePlus.Data.DatabaseModels.Area", "EdgeId");

                    b.HasOne("GeoSensePlus.Data.DatabaseModels.Floorplan", "Floorplan")
                        .WithMany("Areas")
                        .HasForeignKey("FloorplanId");

                    b.Navigation("Edge");

                    b.Navigation("Floorplan");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.CellTag", b =>
                {
                    b.HasOne("GeoSensePlus.Data.DatabaseModels.Edge", null)
                        .WithMany("CellTags")
                        .HasForeignKey("EdgeId");

                    b.HasOne("GeoSensePlus.Data.DatabaseModels.Target", "Target")
                        .WithMany("CellTags")
                        .HasForeignKey("TargetId");

                    b.Navigation("Target");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Edge", b =>
                {
                    b.HasOne("GeoSensePlus.Data.DatabaseModels.Gateway", "Gateway")
                        .WithMany("Edges")
                        .HasForeignKey("GatewayId");

                    b.Navigation("Gateway");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Floorplan", b =>
                {
                    b.HasOne("GeoSensePlus.Data.DatabaseModels.Building", "Building")
                        .WithMany("Floorplans")
                        .HasForeignKey("BuildingId");

                    b.Navigation("Building");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.GpsTag", b =>
                {
                    b.HasOne("GeoSensePlus.Data.DatabaseModels.Target", "Target")
                        .WithMany("GpsTags")
                        .HasForeignKey("TargetId");

                    b.Navigation("Target");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Sensor", b =>
                {
                    b.HasOne("GeoSensePlus.Data.DatabaseModels.Edge", "Edge")
                        .WithMany()
                        .HasForeignKey("EdgeId");

                    b.HasOne("GeoSensePlus.Data.DatabaseModels.Measure", "Measure")
                        .WithMany("Sensors")
                        .HasForeignKey("MeasureId");

                    b.Navigation("Edge");

                    b.Navigation("Measure");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.UwbTag", b =>
                {
                    b.HasOne("GeoSensePlus.Data.DatabaseModels.Target", "Target")
                        .WithMany("UwbTags")
                        .HasForeignKey("TargetId");

                    b.Navigation("Target");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Building", b =>
                {
                    b.Navigation("Floorplans");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Edge", b =>
                {
                    b.Navigation("Area");

                    b.Navigation("CellTags");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Floorplan", b =>
                {
                    b.Navigation("Areas");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Gateway", b =>
                {
                    b.Navigation("Edges");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Measure", b =>
                {
                    b.Navigation("Sensors");
                });

            modelBuilder.Entity("GeoSensePlus.Data.DatabaseModels.Target", b =>
                {
                    b.Navigation("CellTags");

                    b.Navigation("GpsTags");

                    b.Navigation("UwbTags");
                });
#pragma warning restore 612, 618
        }
    }
}
