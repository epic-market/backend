using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenamedCatalogToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogCategories_Businesses_BusinessID",
                table: "CatalogCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_Businesses_BusinessID",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_CatalogCategories_CategoryID",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_StatusOptionSets_StatusId",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogVariants_Catalogs_CatalogID",
                table: "CatalogVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_CatalogVariants_ProductVariantID",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_CatalogVariants_VariantID",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Catalogs_ProductId",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatalogVariants",
                table: "CatalogVariants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Catalogs",
                table: "Catalogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatalogCategories",
                table: "CatalogCategories");

            migrationBuilder.RenameTable(
                name: "CatalogVariants",
                newName: "ProductVariants");

            migrationBuilder.RenameTable(
                name: "Catalogs",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "CatalogCategories",
                newName: "ProductCategory");

            migrationBuilder.RenameColumn(
                name: "CatalogID",
                table: "ProductVariants",
                newName: "ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_CatalogVariants_CatalogID",
                table: "ProductVariants",
                newName: "IX_ProductVariants_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_Catalogs_StatusId",
                table: "Product",
                newName: "IX_Product_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Catalogs_CategoryID",
                table: "Product",
                newName: "IX_Product_CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Catalogs_BusinessID",
                table: "Product",
                newName: "IX_Product_BusinessID");

            migrationBuilder.RenameIndex(
                name: "IX_CatalogCategories_BusinessID",
                table: "ProductCategory",
                newName: "IX_ProductCategory_BusinessID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariants",
                table: "ProductVariants",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_ProductVariants_ProductVariantID",
                table: "Inventory",
                column: "ProductVariantID",
                principalTable: "ProductVariants",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ProductVariants_VariantID",
                table: "OrderDetails",
                column: "VariantID",
                principalTable: "ProductVariants",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Businesses_BusinessID",
                table: "Product",
                column: "BusinessID",
                principalTable: "Businesses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategory_CategoryID",
                table: "Product",
                column: "CategoryID",
                principalTable: "ProductCategory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_StatusOptionSets_StatusId",
                table: "Product",
                column: "StatusId",
                principalTable: "StatusOptionSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Businesses_BusinessID",
                table: "ProductCategory",
                column: "BusinessID",
                principalTable: "Businesses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_Product_ProductID",
                table: "ProductVariants",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Product_ProductId",
                table: "Ratings",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_ProductVariants_ProductVariantID",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductVariants_VariantID",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Businesses_BusinessID",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategory_CategoryID",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_StatusOptionSets_StatusId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Businesses_BusinessID",
                table: "ProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_Product_ProductID",
                table: "ProductVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Product_ProductId",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariants",
                table: "ProductVariants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "ProductVariants",
                newName: "CatalogVariants");

            migrationBuilder.RenameTable(
                name: "ProductCategory",
                newName: "CatalogCategories");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Catalogs");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "CatalogVariants",
                newName: "CatalogID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariants_ProductID",
                table: "CatalogVariants",
                newName: "IX_CatalogVariants_CatalogID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategory_BusinessID",
                table: "CatalogCategories",
                newName: "IX_CatalogCategories_BusinessID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_StatusId",
                table: "Catalogs",
                newName: "IX_Catalogs_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CategoryID",
                table: "Catalogs",
                newName: "IX_Catalogs_CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_BusinessID",
                table: "Catalogs",
                newName: "IX_Catalogs_BusinessID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatalogVariants",
                table: "CatalogVariants",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatalogCategories",
                table: "CatalogCategories",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Catalogs",
                table: "Catalogs",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogCategories_Businesses_BusinessID",
                table: "CatalogCategories",
                column: "BusinessID",
                principalTable: "Businesses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_Businesses_BusinessID",
                table: "Catalogs",
                column: "BusinessID",
                principalTable: "Businesses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_CatalogCategories_CategoryID",
                table: "Catalogs",
                column: "CategoryID",
                principalTable: "CatalogCategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_StatusOptionSets_StatusId",
                table: "Catalogs",
                column: "StatusId",
                principalTable: "StatusOptionSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogVariants_Catalogs_CatalogID",
                table: "CatalogVariants",
                column: "CatalogID",
                principalTable: "Catalogs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_CatalogVariants_ProductVariantID",
                table: "Inventory",
                column: "ProductVariantID",
                principalTable: "CatalogVariants",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_CatalogVariants_VariantID",
                table: "OrderDetails",
                column: "VariantID",
                principalTable: "CatalogVariants",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Catalogs_ProductId",
                table: "Ratings",
                column: "ProductId",
                principalTable: "Catalogs",
                principalColumn: "ID");
        }
    }
}
