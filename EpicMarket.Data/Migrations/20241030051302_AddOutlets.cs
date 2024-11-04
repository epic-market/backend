using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOutlets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Images",
                table: "ProductInternals");

            migrationBuilder.RenameColumn(
                name: "InStock",
                table: "Catalogs",
                newName: "RequiresRefrigeration");

            migrationBuilder.AddColumn<double>(
                name: "CostPrice",
                table: "ProductInternals",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PackedDepth",
                table: "ProductInternals",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PackedHeight",
                table: "ProductInternals",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PackedWidhth",
                table: "ProductInternals",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Rate",
                table: "ProductInternals",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "ProductInternals",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "MaximumStockLevel",
                table: "OutletProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumStockLevel",
                table: "OutletProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityAvailable",
                table: "OutletProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReorderPoint",
                table: "OutletProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "CostPrice",
                table: "Catalogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PackedDepth",
                table: "Catalogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PackedHeight",
                table: "Catalogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PackedWidhth",
                table: "Catalogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Catalogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "ProductInternals");

            migrationBuilder.DropColumn(
                name: "PackedDepth",
                table: "ProductInternals");

            migrationBuilder.DropColumn(
                name: "PackedHeight",
                table: "ProductInternals");

            migrationBuilder.DropColumn(
                name: "PackedWidhth",
                table: "ProductInternals");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "ProductInternals");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "ProductInternals");

            migrationBuilder.DropColumn(
                name: "MaximumStockLevel",
                table: "OutletProducts");

            migrationBuilder.DropColumn(
                name: "MinimumStockLevel",
                table: "OutletProducts");

            migrationBuilder.DropColumn(
                name: "QuantityAvailable",
                table: "OutletProducts");

            migrationBuilder.DropColumn(
                name: "ReorderPoint",
                table: "OutletProducts");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "PackedDepth",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "PackedHeight",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "PackedWidhth",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Catalogs");

            migrationBuilder.RenameColumn(
                name: "RequiresRefrigeration",
                table: "Catalogs",
                newName: "InStock");

            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "ProductInternals",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
