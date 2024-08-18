using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedEntityToComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntityID",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_EntityID",
                table: "Comments",
                column: "EntityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Entity_EntityID",
                table: "Comments",
                column: "EntityID",
                principalTable: "Entity",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Entity_EntityID",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_EntityID",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "EntityID",
                table: "Comments");
        }
    }
}
