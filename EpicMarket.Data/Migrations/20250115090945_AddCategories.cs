using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "F K_OrderDetails_Catalogs_CatalogID",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_CatalogID",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Catalogs");

            migrationBuilder.RenameColumn(
                name: "Attributes",
                table: "ProductVariants",
                newName: "VariantName");

            migrationBuilder.RenameColumn(
                name: "CatalogID",
                table: "OrderDetails",
                newName: "VariantID");

            migrationBuilder.AddColumn<double>(
                name: "CostPrice",
                table: "ProductVariants",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SalePrice",
                table: "ProductVariants",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "VariantAttributes",
                table: "ProductVariants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProductVariantsVariantID",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Catalogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentID = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Categories_Businesses_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "Businesses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductVariantsVariantID",
                table: "OrderDetails",
                column: "ProductVariantsVariantID");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_CategoryID",
                table: "Catalogs",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_BusinessID",
                table: "Categories",
                column: "BusinessID");

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_Categories_CategoryID",
                table: "Catalogs",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ProductVariants_ProductVariantsVariantID",
                table: "OrderDetails",
                column: "ProductVariantsVariantID",
                principalTable: "ProductVariants",
                principalColumn: "VariantID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_Categories_CategoryID",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductVariants_ProductVariantsVariantID",
                table: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductVariantsVariantID",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_CategoryID",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "VariantAttributes",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "ProductVariantsVariantID",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Catalogs");

            migrationBuilder.RenameColumn(
                name: "VariantName",
                table: "ProductVariants",
                newName: "Attributes");

            migrationBuilder.RenameColumn(
                name: "VariantID",
                table: "OrderDetails",
                newName: "CatalogID");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ProductVariants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "Barcode",
                table: "Catalogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Catalogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CostPrice",
                table: "Catalogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Rate",
                table: "Catalogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CatalogID",
                table: "OrderDetails",
                column: "CatalogID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Catalogs_CatalogID",
                table: "OrderDetails",
                column: "CatalogID",
                principalTable: "Catalogs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
