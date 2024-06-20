using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBasemodelToNewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "StatusOptionSets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "StatusOptionSets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "StatusOptionSets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "StatusOptionSets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "ProductInternals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ProductInternals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "ProductInternals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "ProductInternals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "EventCategory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "EventCategory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "EventCategory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "EventCategory",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "StatusOptionSets");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "StatusOptionSets");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "StatusOptionSets");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "StatusOptionSets");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "ProductInternals");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ProductInternals");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ProductInternals");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "ProductInternals");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "EventCategory");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "EventCategory");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EventCategory");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "EventCategory");
        }
    }
}
