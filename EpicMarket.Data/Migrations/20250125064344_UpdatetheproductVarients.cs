using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatetheproductVarients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_CatalogVariants_CatalogVariantsVariantID",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "CatalogVariantsVariantID",
                table: "OrderDetails",
                newName: "CatalogVariantsID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_CatalogVariantsVariantID",
                table: "OrderDetails",
                newName: "IX_OrderDetails_CatalogVariantsID");

            migrationBuilder.RenameColumn(
                name: "VariantID",
                table: "CatalogVariants",
                newName: "ID");

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "CatalogVariants",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "PackedWidhth",
                table: "CatalogVariants",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "PackedHeight",
                table: "CatalogVariants",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "PackedDepth",
                table: "CatalogVariants",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "CompareAtPrice",
                table: "CatalogVariants",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "MaximumOrderQuantity",
                table: "CatalogVariants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumOrderQuantity",
                table: "CatalogVariants",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_CatalogVariants_CatalogVariantsID",
                table: "OrderDetails",
                column: "CatalogVariantsID",
                principalTable: "CatalogVariants",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_CatalogVariants_CatalogVariantsID",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "MaximumOrderQuantity",
                table: "CatalogVariants");

            migrationBuilder.DropColumn(
                name: "MinimumOrderQuantity",
                table: "CatalogVariants");

            migrationBuilder.RenameColumn(
                name: "CatalogVariantsID",
                table: "OrderDetails",
                newName: "CatalogVariantsVariantID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_CatalogVariantsID",
                table: "OrderDetails",
                newName: "IX_OrderDetails_CatalogVariantsVariantID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "CatalogVariants",
                newName: "VariantID");

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "CatalogVariants",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PackedWidhth",
                table: "CatalogVariants",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PackedHeight",
                table: "CatalogVariants",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PackedDepth",
                table: "CatalogVariants",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CompareAtPrice",
                table: "CatalogVariants",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_CatalogVariants_CatalogVariantsVariantID",
                table: "OrderDetails",
                column: "CatalogVariantsVariantID",
                principalTable: "CatalogVariants",
                principalColumn: "VariantID");
        }
    }
}
