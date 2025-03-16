using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class removedTyposandUpdatedTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_CatalogVariants_CatalogVariantsID",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SusbcriptionStatuses_StatusID",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "SupportQuerys");

            migrationBuilder.DropTable(
                name: "SusbcriptionStatuses");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_CatalogVariantsID",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "CatalogVariantsID",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TaskStatusTypes",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "PackedWidhth",
                table: "CatalogVariants",
                newName: "PackedWidth");

            migrationBuilder.RenameColumn(
                name: "IsDefaultVarient",
                table: "CatalogVariants",
                newName: "IsDefaultVariant");

            migrationBuilder.AlterColumn<string>(
                name: "StatusDescription",
                table: "TaskStatusTypes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SubscriptionStatus",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionStatus", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SupportQueries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Query = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TaskTypeID = table.Column<int>(type: "int", nullable: true),
                    TypeofPersonid = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValueSql: "'System'"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportQueries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SupportQueries_PersonTypes_TypeofPersonid",
                        column: x => x.TypeofPersonid,
                        principalTable: "PersonTypes",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_SupportQueries_TaskTypes_TaskTypeID",
                        column: x => x.TaskTypeID,
                        principalTable: "TaskTypes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_VariantID",
                table: "OrderDetails",
                column: "VariantID");

            migrationBuilder.CreateIndex(
                name: "IX_SupportQueries_TaskTypeID",
                table: "SupportQueries",
                column: "TaskTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SupportQueries_TypeofPersonid",
                table: "SupportQueries",
                column: "TypeofPersonid");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_CatalogVariants_VariantID",
                table: "OrderDetails",
                column: "VariantID",
                principalTable: "CatalogVariants",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_SubscriptionStatus_StatusID",
                table: "Subscriptions",
                column: "StatusID",
                principalTable: "SubscriptionStatus",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_CatalogVariants_VariantID",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SubscriptionStatus_StatusID",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "SubscriptionStatus");

            migrationBuilder.DropTable(
                name: "SupportQueries");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_VariantID",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "TaskStatusTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PackedWidth",
                table: "CatalogVariants",
                newName: "PackedWidhth");

            migrationBuilder.RenameColumn(
                name: "IsDefaultVariant",
                table: "CatalogVariants",
                newName: "IsDefaultVarient");

            migrationBuilder.AlterColumn<string>(
                name: "StatusDescription",
                table: "TaskStatusTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CatalogVariantsID",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SupportQuerys",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskTypeID = table.Column<int>(type: "int", nullable: true),
                    TypeofPersonid = table.Column<int>(type: "int", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValueSql: "'System'"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Query = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportQuerys", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SupportQuerys_PersonTypes_TypeofPersonid",
                        column: x => x.TypeofPersonid,
                        principalTable: "PersonTypes",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_SupportQuerys_TaskTypes_TaskTypeID",
                        column: x => x.TaskTypeID,
                        principalTable: "TaskTypes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SusbcriptionStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SusbcriptionStatuses", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CatalogVariantsID",
                table: "OrderDetails",
                column: "CatalogVariantsID");

            migrationBuilder.CreateIndex(
                name: "IX_SupportQuerys_TaskTypeID",
                table: "SupportQuerys",
                column: "TaskTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SupportQuerys_TypeofPersonid",
                table: "SupportQuerys",
                column: "TypeofPersonid");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_CatalogVariants_CatalogVariantsID",
                table: "OrderDetails",
                column: "CatalogVariantsID",
                principalTable: "CatalogVariants",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_SusbcriptionStatuses_StatusID",
                table: "Subscriptions",
                column: "StatusID",
                principalTable: "SusbcriptionStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
