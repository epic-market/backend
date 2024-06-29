using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class _158tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_AspNetUsers_PersonId",
                table: "SupportTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_SupportTicketTypes_TicketTypeID",
                table: "SupportTickets");

            migrationBuilder.DropTable(
                name: "SupportTicketTypes");

            migrationBuilder.DropIndex(
                name: "IX_SupportTickets_PersonId",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "SupportTickets");

            migrationBuilder.RenameColumn(
                name: "TicketTypeID",
                table: "SupportTickets",
                newName: "TypeofPersonid");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "SupportTickets",
                newName: "Phonenumber");

            migrationBuilder.RenameColumn(
                name: "Attachment",
                table: "SupportTickets",
                newName: "Fullname");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_TicketTypeID",
                table: "SupportTickets",
                newName: "IX_SupportTickets_TypeofPersonid");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "SupportTickets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "SupportTickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatusID",
                table: "SupportTickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PersonTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PromotionalLeads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    WhichApplication = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionalLeads", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_AppUserId",
                table: "SupportTickets",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_TaskStatusID",
                table: "SupportTickets",
                column: "TaskStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_AspNetUsers_AppUserId",
                table: "SupportTickets",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_PersonTypes_TypeofPersonid",
                table: "SupportTickets",
                column: "TypeofPersonid",
                principalTable: "PersonTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_TaskStatusTypes_TaskStatusID",
                table: "SupportTickets",
                column: "TaskStatusID",
                principalTable: "TaskStatusTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_AspNetUsers_AppUserId",
                table: "SupportTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_PersonTypes_TypeofPersonid",
                table: "SupportTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_TaskStatusTypes_TaskStatusID",
                table: "SupportTickets");

            migrationBuilder.DropTable(
                name: "PersonTypes");

            migrationBuilder.DropTable(
                name: "PromotionalLeads");

            migrationBuilder.DropIndex(
                name: "IX_SupportTickets_AppUserId",
                table: "SupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_SupportTickets_TaskStatusID",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "TaskStatusID",
                table: "SupportTickets");

            migrationBuilder.RenameColumn(
                name: "TypeofPersonid",
                table: "SupportTickets",
                newName: "TicketTypeID");

            migrationBuilder.RenameColumn(
                name: "Phonenumber",
                table: "SupportTickets",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Fullname",
                table: "SupportTickets",
                newName: "Attachment");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_TypeofPersonid",
                table: "SupportTickets",
                newName: "IX_SupportTickets_TicketTypeID");

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "SupportTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SupportTicketTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicketTypes", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_PersonId",
                table: "SupportTickets",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_AspNetUsers_PersonId",
                table: "SupportTickets",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_SupportTicketTypes_TicketTypeID",
                table: "SupportTickets",
                column: "TicketTypeID",
                principalTable: "SupportTicketTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
