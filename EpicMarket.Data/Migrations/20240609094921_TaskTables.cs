using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class TaskTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TaskCategoryID = table.Column<int>(type: "int", nullable: true),
                    DefaultDueDateHours = table.Column<int>(type: "int", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskTypes_EventCategory_TaskCategoryID",
                        column: x => x.TaskCategoryID,
                        principalTable: "EventCategory",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Taskss",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskTypeID = table.Column<int>(type: "int", nullable: true),
                    TaskStatusID = table.Column<int>(type: "int", nullable: true),
                    TaskPriorityID = table.Column<int>(type: "int", nullable: true),
                    PrimaryAssignedToPersonID = table.Column<int>(type: "int", nullable: true),
                    DateAssigned = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDue = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateStarted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmittedByPersonID = table.Column<int>(type: "int", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TaskData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taskss", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Taskss_StatusOptionSets_TaskStatusID",
                        column: x => x.TaskStatusID,
                        principalTable: "StatusOptionSets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Taskss_TaskTypes_TaskTypeID",
                        column: x => x.TaskTypeID,
                        principalTable: "TaskTypes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Taskss_TaskStatusID",
                table: "Taskss",
                column: "TaskStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Taskss_TaskTypeID",
                table: "Taskss",
                column: "TaskTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypes_TaskCategoryID",
                table: "TaskTypes",
                column: "TaskCategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Taskss");

            migrationBuilder.DropTable(
                name: "TaskTypes");
        }
    }
}
