using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenanamedTheCatalogCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_Categories_CategoryID",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Businesses_BusinessID",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "CatalogCategories");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_BusinessID",
                table: "CatalogCategories",
                newName: "IX_CatalogCategories_BusinessID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatalogCategories",
                table: "CatalogCategories",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogCategories_Businesses_BusinessID",
                table: "CatalogCategories",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogCategories_Businesses_BusinessID",
                table: "CatalogCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_CatalogCategories_CategoryID",
                table: "Catalogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatalogCategories",
                table: "CatalogCategories");

            migrationBuilder.RenameTable(
                name: "CatalogCategories",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_CatalogCategories_BusinessID",
                table: "Categories",
                newName: "IX_Categories_BusinessID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_Categories_CategoryID",
                table: "Catalogs",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Businesses_BusinessID",
                table: "Categories",
                column: "BusinessID",
                principalTable: "Businesses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
