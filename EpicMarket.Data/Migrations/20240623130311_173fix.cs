using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class _173fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "TaskEntityID",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskEntityID",
                table: "Tasks",
                column: "TaskEntityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Entity_TaskEntityID",
                table: "Tasks",
                column: "TaskEntityID",
                principalTable: "Entity",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Entity_TaskEntityID",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskEntityID",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskEntityID",
                table: "Tasks");

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                table: "Tasks",
                type: "datetime2",
                nullable: true);
        }
    }
}
