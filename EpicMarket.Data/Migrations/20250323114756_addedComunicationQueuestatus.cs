using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedComunicationQueuestatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Attempts",
                table: "CommunicationQueue",
                newName: "CommunicationStatusId");

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "CommunicationQueue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                table: "CommunicationQueue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentDate",
                table: "CommunicationQueue",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                table: "CommunicationQueue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommunicationStatus",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationStatus", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationQueue_CommunicationStatusId",
                table: "CommunicationQueue",
                column: "CommunicationStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunicationQueue_CommunicationStatus_CommunicationStatusId",
                table: "CommunicationQueue",
                column: "CommunicationStatusId",
                principalTable: "CommunicationStatus",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunicationQueue_CommunicationStatus_CommunicationStatusId",
                table: "CommunicationQueue");

            migrationBuilder.DropTable(
                name: "CommunicationStatus");

            migrationBuilder.DropIndex(
                name: "IX_CommunicationQueue_CommunicationStatusId",
                table: "CommunicationQueue");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "CommunicationQueue");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "CommunicationQueue");

            migrationBuilder.DropColumn(
                name: "SentDate",
                table: "CommunicationQueue");

            migrationBuilder.DropColumn(
                name: "TemplateName",
                table: "CommunicationQueue");

            migrationBuilder.RenameColumn(
                name: "CommunicationStatusId",
                table: "CommunicationQueue",
                newName: "Attempts");
        }
    }
}
