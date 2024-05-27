using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class Outletaddorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Outlets_Outlets_OutletID",
                table: "Outlets");

            migrationBuilder.DropIndex(
                name: "IX_Outlets_OutletID",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "OutletID",
                table: "Outlets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OutletID",
                table: "Outlets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Outlets_OutletID",
                table: "Outlets",
                column: "OutletID");

            migrationBuilder.AddForeignKey(
                name: "FK_Outlets_Outlets_OutletID",
                table: "Outlets",
                column: "OutletID",
                principalTable: "Outlets",
                principalColumn: "ID");
        }
    }
}
