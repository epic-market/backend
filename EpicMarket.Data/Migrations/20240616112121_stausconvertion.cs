using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class stausconvertion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Catalogs");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Outlets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderTypeId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Catalogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CatalogStatusOptionSet",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    StatusOptionSetsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogStatusOptionSet", x => new { x.StatusId, x.StatusOptionSetsId });
                    table.ForeignKey(
                        name: "FK_CatalogStatusOptionSet_Catalogs_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Catalogs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogStatusOptionSet_StatusOptionSets_StatusOptionSetsId",
                        column: x => x.StatusOptionSetsId,
                        principalTable: "StatusOptionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderTypesOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ordertype = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTypesOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutletStatusOptionSet",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    StatusOptionSetsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutletStatusOptionSet", x => new { x.StatusId, x.StatusOptionSetsId });
                    table.ForeignKey(
                        name: "FK_OutletStatusOptionSet_Outlets_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Outlets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutletStatusOptionSet_StatusOptionSets_StatusOptionSetsId",
                        column: x => x.StatusOptionSetsId,
                        principalTable: "StatusOptionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderTypeId",
                table: "Orders",
                column: "OrderTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusId",
                table: "Orders",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogStatusOptionSet_StatusOptionSetsId",
                table: "CatalogStatusOptionSet",
                column: "StatusOptionSetsId");

            migrationBuilder.CreateIndex(
                name: "IX_OutletStatusOptionSet_StatusOptionSetsId",
                table: "OutletStatusOptionSet",
                column: "StatusOptionSetsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderStatusOptions_StatusId",
                table: "Orders",
                column: "StatusId",
                principalTable: "OrderStatusOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderTypesOptions_OrderTypeId",
                table: "Orders",
                column: "OrderTypeId",
                principalTable: "OrderTypesOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderStatusOptions_StatusId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderTypesOptions_OrderTypeId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "CatalogStatusOptionSet");

            migrationBuilder.DropTable(
                name: "OrderTypesOptions");

            migrationBuilder.DropTable(
                name: "OutletStatusOptionSet");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderTypeId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_StatusId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "OrderTypeId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Catalogs");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Outlets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderType",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Catalogs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
