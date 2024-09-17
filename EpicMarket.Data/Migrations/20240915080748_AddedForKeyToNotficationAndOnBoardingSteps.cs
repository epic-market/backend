using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedForKeyToNotficationAndOnBoardingSteps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NavigationURL",
                table: "OnboardingSteps");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "QuickLinkId",
                table: "OnboardingSteps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuickLinkId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingSteps_QuickLinkId",
                table: "OnboardingSteps",
                column: "QuickLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_QuickLinkId",
                table: "Notifications",
                column: "QuickLinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Quicklink_QuickLinkId",
                table: "Notifications",
                column: "QuickLinkId",
                principalTable: "Quicklink",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnboardingSteps_Quicklink_QuickLinkId",
                table: "OnboardingSteps",
                column: "QuickLinkId",
                principalTable: "Quicklink",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Quicklink_QuickLinkId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_OnboardingSteps_Quicklink_QuickLinkId",
                table: "OnboardingSteps");

            migrationBuilder.DropIndex(
                name: "IX_OnboardingSteps_QuickLinkId",
                table: "OnboardingSteps");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_QuickLinkId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "QuickLinkId",
                table: "OnboardingSteps");

            migrationBuilder.DropColumn(
                name: "QuickLinkId",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "NavigationURL",
                table: "OnboardingSteps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
