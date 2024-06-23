using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedthestatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogStatusOptionSet");

            migrationBuilder.DropTable(
                name: "OutletStatusOptionSet");

            migrationBuilder.CreateIndex(
                name: "IX_Outlets_StatusId",
                table: "Outlets",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_StatusId",
                table: "Catalogs",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_StatusOptionSets_StatusId",
                table: "Catalogs",
                column: "StatusId",
                principalTable: "StatusOptionSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Outlets_StatusOptionSets_StatusId",
                table: "Outlets",
                column: "StatusId",
                principalTable: "StatusOptionSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_StatusOptionSets_StatusId",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Outlets_StatusOptionSets_StatusId",
                table: "Outlets");

            migrationBuilder.DropIndex(
                name: "IX_Outlets_StatusId",
                table: "Outlets");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_StatusId",
                table: "Catalogs");

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
                name: "IX_CatalogStatusOptionSet_StatusOptionSetsId",
                table: "CatalogStatusOptionSet",
                column: "StatusOptionSetsId");

            migrationBuilder.CreateIndex(
                name: "IX_OutletStatusOptionSet_StatusOptionSetsId",
                table: "OutletStatusOptionSet",
                column: "StatusOptionSetsId");
        }
    }
}
