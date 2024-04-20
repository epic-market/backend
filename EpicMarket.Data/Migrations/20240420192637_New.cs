using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutletPeople_Outlets_PersonId",
                table: "OutletPeople");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_OutletPeople_OutletId",
                table: "OutletPeople",
                column: "OutletId");

            migrationBuilder.AddForeignKey(
                name: "FK_OutletPeople_Outlets_OutletId",
                table: "OutletPeople",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutletPeople_Outlets_OutletId",
                table: "OutletPeople");

            migrationBuilder.DropIndex(
                name: "IX_OutletPeople_OutletId",
                table: "OutletPeople");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OutletPeople_Outlets_PersonId",
                table: "OutletPeople",
                column: "PersonId",
                principalTable: "Outlets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
