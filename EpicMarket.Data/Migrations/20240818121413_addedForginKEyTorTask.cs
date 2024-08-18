using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedForginKEyTorTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PrimaryAssignedToPersonID",
                table: "Tasks",
                column: "PrimaryAssignedToPersonID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_PrimaryAssignedToPersonID",
                table: "Tasks",
                column: "PrimaryAssignedToPersonID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_PrimaryAssignedToPersonID",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_PrimaryAssignedToPersonID",
                table: "Tasks");
        }
    }
}
