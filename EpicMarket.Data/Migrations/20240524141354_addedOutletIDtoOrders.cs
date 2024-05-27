using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedOutletIDtoOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Businesses_BusinessID",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "BusinessID",
                table: "Orders",
                newName: "OutletID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_BusinessID",
                table: "Orders",
                newName: "IX_Orders_OutletID");

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
                name: "FK_Orders_Outlets_OutletID",
                table: "Orders",
                column: "OutletID",
                principalTable: "Outlets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Outlets_Outlets_OutletID",
                table: "Outlets",
                column: "OutletID",
                principalTable: "Outlets",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Outlets_OutletID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Outlets_Outlets_OutletID",
                table: "Outlets");

            migrationBuilder.DropIndex(
                name: "IX_Outlets_OutletID",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "OutletID",
                table: "Outlets");

            migrationBuilder.RenameColumn(
                name: "OutletID",
                table: "Orders",
                newName: "BusinessID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_OutletID",
                table: "Orders",
                newName: "IX_Orders_BusinessID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Businesses_BusinessID",
                table: "Orders",
                column: "BusinessID",
                principalTable: "Businesses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
