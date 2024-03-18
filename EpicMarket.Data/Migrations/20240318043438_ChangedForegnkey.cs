using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedForegnkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutletProducts_Outlets_ProductID",
                table: "OutletProducts");

            migrationBuilder.CreateIndex(
                name: "IX_OutletProducts_OutletID",
                table: "OutletProducts",
                column: "OutletID");

            migrationBuilder.AddForeignKey(
                name: "FK_OutletProducts_Outlets_OutletID",
                table: "OutletProducts",
                column: "OutletID",
                principalTable: "Outlets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutletProducts_Outlets_OutletID",
                table: "OutletProducts");

            migrationBuilder.DropIndex(
                name: "IX_OutletProducts_OutletID",
                table: "OutletProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_OutletProducts_Outlets_ProductID",
                table: "OutletProducts",
                column: "ProductID",
                principalTable: "Outlets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
