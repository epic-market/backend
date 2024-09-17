using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedBasemodeltoPages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Pages",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Pages",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Pages");
        }
    }
}
