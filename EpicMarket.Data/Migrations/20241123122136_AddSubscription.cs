using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_AspNetUsers_CustomerId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Catalogs_ProductId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Outlets_OutletId",
                table: "Rating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rating",
                table: "Rating");

            migrationBuilder.RenameTable(
                name: "Rating",
                newName: "Ratings");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_ProductId",
                table: "Ratings",
                newName: "IX_Ratings_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_OutletId",
                table: "Ratings",
                newName: "IX_Ratings_OutletId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_CustomerId",
                table: "Ratings",
                newName: "IX_Ratings_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SusbcriptionStatus",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SusbcriptionStatus", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OutletId = table.Column<int>(type: "int", nullable: false),
                    SubscribedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnsubscribedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Outlets_OutletId",
                        column: x => x.OutletId,
                        principalTable: "Outlets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_SusbcriptionStatus_StatusID",
                        column: x => x.StatusID,
                        principalTable: "SusbcriptionStatus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_CustomerId",
                table: "Subscriptions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_OutletId",
                table: "Subscriptions",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_StatusID",
                table: "Subscriptions",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_CustomerId",
                table: "Ratings",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Catalogs_ProductId",
                table: "Ratings",
                column: "ProductId",
                principalTable: "Catalogs",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Outlets_OutletId",
                table: "Ratings",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_CustomerId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Catalogs_ProductId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Outlets_OutletId",
                table: "Ratings");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "SusbcriptionStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.RenameTable(
                name: "Ratings",
                newName: "Rating");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_ProductId",
                table: "Rating",
                newName: "IX_Rating_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_OutletId",
                table: "Rating",
                newName: "IX_Rating_OutletId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_CustomerId",
                table: "Rating",
                newName: "IX_Rating_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rating",
                table: "Rating",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_AspNetUsers_CustomerId",
                table: "Rating",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Catalogs_ProductId",
                table: "Rating",
                column: "ProductId",
                principalTable: "Catalogs",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Outlets_OutletId",
                table: "Rating",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "ID");
        }
    }
}
