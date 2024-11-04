using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedTheQuickLinksAddedToPages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Quicklink_QuickLinkId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_OnboardingSteps_Quicklink_QuickLinkId",
                table: "OnboardingSteps");

            migrationBuilder.DropTable(
                name: "Quicklink");

            migrationBuilder.RenameColumn(
                name: "backOrders",
                table: "OutletProducts",
                newName: "BackOrders");

            migrationBuilder.RenameColumn(
                name: "QuickLinkId",
                table: "OnboardingSteps",
                newName: "PageId");

            migrationBuilder.RenameIndex(
                name: "IX_OnboardingSteps_QuickLinkId",
                table: "OnboardingSteps",
                newName: "IX_OnboardingSteps_PageId");

            migrationBuilder.RenameColumn(
                name: "QuickLinkId",
                table: "Notifications",
                newName: "PageId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_QuickLinkId",
                table: "Notifications",
                newName: "IX_Notifications_PageId");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Pages_PageId",
                table: "Notifications",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnboardingSteps_Pages_PageId",
                table: "OnboardingSteps",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Pages_PageId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_OnboardingSteps_Pages_PageId",
                table: "OnboardingSteps");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Pages");

            migrationBuilder.RenameColumn(
                name: "BackOrders",
                table: "OutletProducts",
                newName: "backOrders");

            migrationBuilder.RenameColumn(
                name: "PageId",
                table: "OnboardingSteps",
                newName: "QuickLinkId");

            migrationBuilder.RenameIndex(
                name: "IX_OnboardingSteps_PageId",
                table: "OnboardingSteps",
                newName: "IX_OnboardingSteps_QuickLinkId");

            migrationBuilder.RenameColumn(
                name: "PageId",
                table: "Notifications",
                newName: "QuickLinkId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_PageId",
                table: "Notifications",
                newName: "IX_Notifications_QuickLinkId");

            migrationBuilder.CreateTable(
                name: "Quicklink",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValueSql: "'System'"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quicklink", x => x.Id);
                });

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
    }
}
