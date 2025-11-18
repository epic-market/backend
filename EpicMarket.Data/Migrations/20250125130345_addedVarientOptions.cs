using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedVarientOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionName",
                table: "CatalogVariants");

            migrationBuilder.DropColumn(
                name: "OptionValue",
                table: "CatalogVariants");

            migrationBuilder.AddColumn<string>(
                name: "Attributes",
                table: "CatalogVariants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VarientOptions",
                table: "Catalogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attributes",
                table: "CatalogVariants");

            migrationBuilder.DropColumn(
                name: "VarientOptions",
                table: "Catalogs");

            migrationBuilder.AddColumn<string>(
                name: "OptionName",
                table: "CatalogVariants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OptionValue",
                table: "CatalogVariants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
