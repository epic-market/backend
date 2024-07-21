using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedSupportQuerys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeofPersonid",
                table: "SupportQuerys",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportQuerys_TypeofPersonid",
                table: "SupportQuerys",
                column: "TypeofPersonid");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportQuerys_PersonTypes_TypeofPersonid",
                table: "SupportQuerys",
                column: "TypeofPersonid",
                principalTable: "PersonTypes",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportQuerys_PersonTypes_TypeofPersonid",
                table: "SupportQuerys");

            migrationBuilder.DropIndex(
                name: "IX_SupportQuerys_TypeofPersonid",
                table: "SupportQuerys");

            migrationBuilder.DropColumn(
                name: "TypeofPersonid",
                table: "SupportQuerys");
        }
    }
}
