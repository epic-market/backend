using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedInActiveToUserAndBusinessEmployeeMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "BusinessEmployeeMaps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "BusinessEmployeeMaps",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BusinessEmployeeMaps",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "BusinessEmployeeMaps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "BusinessEmployeeMaps",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "BusinessEmployeeMaps");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "BusinessEmployeeMaps");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BusinessEmployeeMaps");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "BusinessEmployeeMaps");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "BusinessEmployeeMaps");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");
        }
    }
}
