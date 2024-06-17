using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class Tasktableadder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Taskss_TaskStatusTypes_TaskStatusID",
                table: "Taskss");

            migrationBuilder.DropForeignKey(
                name: "FK_Taskss_TaskTypes_TaskTypeID",
                table: "Taskss");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Taskss",
                table: "Taskss");

            migrationBuilder.RenameTable(
                name: "Taskss",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_Taskss_TaskTypeID",
                table: "Tasks",
                newName: "IX_Tasks_TaskTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Taskss_TaskStatusID",
                table: "Tasks",
                newName: "IX_Tasks_TaskStatusID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskStatusTypes_TaskStatusID",
                table: "Tasks",
                column: "TaskStatusID",
                principalTable: "TaskStatusTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskTypes_TaskTypeID",
                table: "Tasks",
                column: "TaskTypeID",
                principalTable: "TaskTypes",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskStatusTypes_TaskStatusID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskTypes_TaskTypeID",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Taskss");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_TaskTypeID",
                table: "Taskss",
                newName: "IX_Taskss_TaskTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_TaskStatusID",
                table: "Taskss",
                newName: "IX_Taskss_TaskStatusID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Taskss",
                table: "Taskss",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Taskss_TaskStatusTypes_TaskStatusID",
                table: "Taskss",
                column: "TaskStatusID",
                principalTable: "TaskStatusTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Taskss_TaskTypes_TaskTypeID",
                table: "Taskss",
                column: "TaskTypeID",
                principalTable: "TaskTypes",
                principalColumn: "ID");
        }
    }
}
