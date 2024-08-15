using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedTheAttachmentTypeFromAttactMentAddedInAttachmentLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_AttachmentTypes_AttachmentTypeID",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_AttachmentTypeID",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "AttachmentTypeID",
                table: "Attachments");

            migrationBuilder.AddColumn<int>(
                name: "AttachmentTypeID",
                table: "AttachmentLinks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentLinks_AttachmentTypeID",
                table: "AttachmentLinks",
                column: "AttachmentTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentLinks_AttachmentTypes_AttachmentTypeID",
                table: "AttachmentLinks",
                column: "AttachmentTypeID",
                principalTable: "AttachmentTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttachmentLinks_AttachmentTypes_AttachmentTypeID",
                table: "AttachmentLinks");

            migrationBuilder.DropIndex(
                name: "IX_AttachmentLinks_AttachmentTypeID",
                table: "AttachmentLinks");

            migrationBuilder.DropColumn(
                name: "AttachmentTypeID",
                table: "AttachmentLinks");

            migrationBuilder.AddColumn<int>(
                name: "AttachmentTypeID",
                table: "Attachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_AttachmentTypeID",
                table: "Attachments",
                column: "AttachmentTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_AttachmentTypes_AttachmentTypeID",
                table: "Attachments",
                column: "AttachmentTypeID",
                principalTable: "AttachmentTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
