using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOutletFinaceTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Finances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutletID = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Finances_Outlets_OutletID",
                        column: x => x.OutletID,
                        principalTable: "Outlets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MerchantBankAccounts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantFinanceId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IfscCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountHolderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPrimaryAccount = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantBankAccounts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MerchantBankAccounts_Finances_MerchantFinanceId",
                        column: x => x.MerchantFinanceId,
                        principalTable: "Finances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MerchantUpiAccounts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantFinanceId = table.Column<int>(type: "int", nullable: false),
                    UpiIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerchantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrCodeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPrimaryAccount = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantUpiAccounts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MerchantUpiAccounts_Finances_MerchantFinanceId",
                        column: x => x.MerchantFinanceId,
                        principalTable: "Finances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Finances_OutletID",
                table: "Finances",
                column: "OutletID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MerchantBankAccounts_MerchantFinanceId",
                table: "MerchantBankAccounts",
                column: "MerchantFinanceId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantUpiAccounts_MerchantFinanceId",
                table: "MerchantUpiAccounts",
                column: "MerchantFinanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MerchantBankAccounts");

            migrationBuilder.DropTable(
                name: "MerchantUpiAccounts");

            migrationBuilder.DropTable(
                name: "Finances");
        }
    }
}
