using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedEntityRecordIdForAttachemnt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntityId",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecordId",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_EntityId",
                table: "Attachments",
                column: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Entity_EntityId",
                table: "Attachments",
                column: "EntityId",
                principalTable: "Entity",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Entity_EntityId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_EntityId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "RecordId",
                table: "Attachments");
        }
    }
}
