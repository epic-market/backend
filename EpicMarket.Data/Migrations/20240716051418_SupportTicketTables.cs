using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class SupportTicketTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_TaskStatusTypes_TaskStatusID",
                table: "SupportTickets");

            migrationBuilder.RenameColumn(
                name: "TaskStatusID",
                table: "SupportTickets",
                newName: "Taskid");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_TaskStatusID",
                table: "SupportTickets",
                newName: "IX_SupportTickets_Taskid");

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "SupportTickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "SupportTickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "SupportTickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "SupportTickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SupportQuerys",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Query = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskTypeID = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportQuerys", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SupportQuerys_TaskTypes_TaskTypeID",
                        column: x => x.TaskTypeID,
                        principalTable: "TaskTypes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportQuerys_TaskTypeID",
                table: "SupportQuerys",
                column: "TaskTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_Tasks_Taskid",
                table: "SupportTickets",
                column: "Taskid",
                principalTable: "Tasks",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_Tasks_Taskid",
                table: "SupportTickets");

            migrationBuilder.DropTable(
                name: "SupportQuerys");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "SupportTickets");

            migrationBuilder.RenameColumn(
                name: "Taskid",
                table: "SupportTickets",
                newName: "TaskStatusID");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_Taskid",
                table: "SupportTickets",
                newName: "IX_SupportTickets_TaskStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_TaskStatusTypes_TaskStatusID",
                table: "SupportTickets",
                column: "TaskStatusID",
                principalTable: "TaskStatusTypes",
                principalColumn: "Id");
        }
    }
}
