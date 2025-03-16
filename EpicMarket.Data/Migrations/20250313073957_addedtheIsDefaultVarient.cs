using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedtheIsDefaultVarient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskStatusTypes_TaskStatusID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskTypes_TaskTypeID",
                table: "Tasks");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultVarient",
                table: "CatalogVariants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskStatusTypes_TaskStatusID",
                table: "Tasks",
                column: "TaskStatusID",
                principalTable: "TaskStatusTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskTypes_TaskTypeID",
                table: "Tasks",
                column: "TaskTypeID",
                principalTable: "TaskTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.DropColumn(
                name: "IsDefaultVarient",
                table: "CatalogVariants");

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
    }
}
