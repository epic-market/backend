using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BlogCategory_BlogCategoryID",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_EventCategory_Event_EventID",
                table: "EventCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_EventLog_EventType_EventTypeID",
                table: "EventLog");

            migrationBuilder.DropTable(
                name: "EventType");

            migrationBuilder.DropIndex(
                name: "IX_EventLog_EventTypeID",
                table: "EventLog");

            migrationBuilder.DropIndex(
                name: "IX_EventCategory_EventID",
                table: "EventCategory");

            migrationBuilder.DropColumn(
                name: "EventTypeID",
                table: "EventLog");

            migrationBuilder.DropColumn(
                name: "NotificationQueueDate",
                table: "EventLog");

            migrationBuilder.DropColumn(
                name: "SessionID",
                table: "EventLog");

            migrationBuilder.DropColumn(
                name: "AlertIcons",
                table: "EventCategory");

            migrationBuilder.DropColumn(
                name: "EventCategoryIcon",
                table: "EventCategory");

            migrationBuilder.DropColumn(
                name: "EventID",
                table: "EventCategory");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EventCategory");

            migrationBuilder.DropColumn(
                name: "IsShownInAlerts",
                table: "EventCategory");

            migrationBuilder.DropColumn(
                name: "EventTypeID",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "HelpText",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "IsShownOnScreen",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "IsAudited",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "ContactMethod");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "ContactMethod");

            migrationBuilder.DropColumn(
                name: "EventLogID",
                table: "CommunicationQueue");

            migrationBuilder.AddColumn<int>(
                name: "EntityID",
                table: "EventLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecordID",
                table: "EventLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EventLog_EntityID",
                table: "EventLog",
                column: "EntityID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_EventCategoryID",
                table: "Event",
                column: "EventCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BlogCategory_BlogCategoryID",
                table: "Blogs",
                column: "BlogCategoryID",
                principalTable: "BlogCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_EventCategory_EventCategoryID",
                table: "Event",
                column: "EventCategoryID",
                principalTable: "EventCategory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventLog_Entity_EntityID",
                table: "EventLog",
                column: "EntityID",
                principalTable: "Entity",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BlogCategory_BlogCategoryID",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_EventCategory_EventCategoryID",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_EventLog_Entity_EntityID",
                table: "EventLog");

            migrationBuilder.DropIndex(
                name: "IX_EventLog_EntityID",
                table: "EventLog");

            migrationBuilder.DropIndex(
                name: "IX_Event_EventCategoryID",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "EntityID",
                table: "EventLog");

            migrationBuilder.DropColumn(
                name: "RecordID",
                table: "EventLog");

            migrationBuilder.AddColumn<int>(
                name: "EventTypeID",
                table: "EventLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NotificationQueueDate",
                table: "EventLog",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionID",
                table: "EventLog",
                type: "nvarchar(88)",
                maxLength: 88,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlertIcons",
                table: "EventCategory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventCategoryIcon",
                table: "EventCategory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventID",
                table: "EventCategory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EventCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsShownInAlerts",
                table: "EventCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EventTypeID",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HelpText",
                table: "Event",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Event",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsShownOnScreen",
                table: "Event",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAudited",
                table: "Entity",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "ContactMethod",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "ContactMethod",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EventLogID",
                table: "CommunicationQueue",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EventID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventType", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EventType_Event_EventID",
                        column: x => x.EventID,
                        principalTable: "Event",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventLog_EventTypeID",
                table: "EventLog",
                column: "EventTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_EventCategory_EventID",
                table: "EventCategory",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_EventType_EventID",
                table: "EventType",
                column: "EventID");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BlogCategory_BlogCategoryID",
                table: "Blogs",
                column: "BlogCategoryID",
                principalTable: "BlogCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventCategory_Event_EventID",
                table: "EventCategory",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_EventLog_EventType_EventTypeID",
                table: "EventLog",
                column: "EventTypeID",
                principalTable: "EventType",
                principalColumn: "ID");
        }
    }
}
