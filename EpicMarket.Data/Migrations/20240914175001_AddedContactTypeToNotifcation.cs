using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedContactTypeToNotifcation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "ContactMethodId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ContactMethodId",
                table: "Notifications",
                column: "ContactMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_ContactMethod_ContactMethodId",
                table: "Notifications",
                column: "ContactMethodId",
                principalTable: "ContactMethod",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_ContactMethod_ContactMethodId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ContactMethodId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ContactMethodId",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
