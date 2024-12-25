using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedProofTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SusbcriptionStatus_StatusID",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SusbcriptionStatus",
                table: "SusbcriptionStatus");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Subscriptions");

            migrationBuilder.RenameTable(
                name: "SusbcriptionStatus",
                newName: "SusbcriptionStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SusbcriptionStatuses",
                table: "SusbcriptionStatuses",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "ProofTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proofs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    ProofNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proofs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proofs_ProofTypes_ProofTypeId",
                        column: x => x.ProofTypeId,
                        principalTable: "ProofTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proofs_ProofTypeId",
                table: "Proofs",
                column: "ProofTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_SusbcriptionStatuses_StatusID",
                table: "Subscriptions",
                column: "StatusID",
                principalTable: "SusbcriptionStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SusbcriptionStatuses_StatusID",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Proofs");

            migrationBuilder.DropTable(
                name: "ProofTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SusbcriptionStatuses",
                table: "SusbcriptionStatuses");

            migrationBuilder.RenameTable(
                name: "SusbcriptionStatuses",
                newName: "SusbcriptionStatus");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SusbcriptionStatus",
                table: "SusbcriptionStatus",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_SusbcriptionStatus_StatusID",
                table: "Subscriptions",
                column: "StatusID",
                principalTable: "SusbcriptionStatus",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
