using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class statusoption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Businesses");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Businesses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StatusOptionSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusOptionSets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_StatusId",
                table: "Businesses",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_StatusOptionSets_StatusId",
                table: "Businesses",
                column: "StatusId",
                principalTable: "StatusOptionSets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_StatusOptionSets_StatusId",
                table: "Businesses");

            migrationBuilder.DropTable(
                name: "StatusOptionSets");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_StatusId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Businesses");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Businesses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
