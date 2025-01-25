using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedReqiredFeildForAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_ProductVariants_ProductVariantID",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductVariants_ProductVariantsVariantID",
                table: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "MaximumOrderPurchase",
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
                name: "ProductVariantsVariantID",
                table: "OrderDetails",
                newName: "CatalogVariantsVariantID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductVariantsVariantID",
                table: "OrderDetails",
                newName: "IX_OrderDetails_CatalogVariantsVariantID");

            migrationBuilder.AlterColumn<int>(
                name: "ReorderPoint",
                table: "Inventory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "QuantityAvailable",
                table: "Inventory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MinimumStockLevel",
                table: "Inventory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MaximumStockLevel",
                table: "Inventory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsInStock",
                table: "Inventory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TrackInventory",
                table: "Inventory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BaseHightlights",
                table: "Catalogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddressID",
                table: "Businesses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "CatalogVariants",
                columns: table => new
                {
                    VariantID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatalogID = table.Column<int>(type: "int", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostPrice = table.Column<double>(type: "float", nullable: false),
                    SalePrice = table.Column<double>(type: "float", nullable: false),
                    CompareAtPrice = table.Column<double>(type: "float", nullable: false),
                    AdditionalHightlights = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackedHeight = table.Column<double>(type: "float", nullable: false),
                    PackedWidhth = table.Column<double>(type: "float", nullable: false),
                    PackedDepth = table.Column<double>(type: "float", nullable: false),
                    WeightUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValueSql: "'System'"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogVariants", x => x.VariantID);
                    table.ForeignKey(
                        name: "FK_CatalogVariants_Catalogs_CatalogID",
                        column: x => x.CatalogID,
                        principalTable: "Catalogs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogVariants_CatalogID",
                table: "CatalogVariants",
                column: "CatalogID");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_CatalogVariants_ProductVariantID",
                table: "Inventory",
                column: "ProductVariantID",
                principalTable: "CatalogVariants",
                principalColumn: "VariantID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_CatalogVariants_CatalogVariantsVariantID",
                table: "OrderDetails",
                column: "CatalogVariantsVariantID",
                principalTable: "CatalogVariants",
                principalColumn: "VariantID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_CatalogVariants_ProductVariantID",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_CatalogVariants_CatalogVariantsVariantID",
                table: "OrderDetails");

            migrationBuilder.DropTable(
                name: "CatalogVariants");

            migrationBuilder.DropColumn(
                name: "IsInStock",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "TrackInventory",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "BaseHightlights",
                table: "Catalogs");

            migrationBuilder.RenameColumn(
                name: "CatalogVariantsVariantID",
                table: "OrderDetails",
                newName: "ProductVariantsVariantID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_CatalogVariantsVariantID",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductVariantsVariantID");

            migrationBuilder.AlterColumn<int>(
                name: "ReorderPoint",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuantityAvailable",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MinimumStockLevel",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaximumStockLevel",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximumOrderPurchase",
                table: "Catalogs",
                type: "int",
                nullable: true);

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

            migrationBuilder.AlterColumn<int>(
                name: "AddressID",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    VariantID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    CostPrice = table.Column<double>(type: "float", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValueSql: "'System'"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SalePrice = table.Column<double>(type: "float", nullable: false),
                    VariantAttributes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VariantName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.VariantID);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Catalogs_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Catalogs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductID",
                table: "ProductVariants",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_ProductVariants_ProductVariantID",
                table: "Inventory",
                column: "ProductVariantID",
                principalTable: "ProductVariants",
                principalColumn: "VariantID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ProductVariants_ProductVariantsVariantID",
                table: "OrderDetails",
                column: "ProductVariantsVariantID",
                principalTable: "ProductVariants",
                principalColumn: "VariantID");
        }
    }
}
