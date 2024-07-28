using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedInActiveAndUserNameUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TaskTypes",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TaskStatusTypes",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tasks",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SupportTickets",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SupportQuerys",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "StatusOptionSets",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProductInternals",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Outlets",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "OrderDetails",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "HelpItems",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FAQs",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FAQCategories",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EventLog",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Event",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Entity",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ContactMethod",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CommunicationQueue",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Businesses",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BusinessCategories",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Blogs",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BlogCategory",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Attachments",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AttachmentLinks",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ApplicationsTable",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ApplicationSecurables",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ApplicationConfigurations",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Addresses",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TaskTypes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TaskStatusTypes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SupportQuerys");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "StatusOptionSets");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProductInternals");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "HelpItems");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FAQs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FAQCategories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EventLog");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ContactMethod");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CommunicationQueue");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BusinessCategories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BlogCategory");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AttachmentLinks");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ApplicationsTable");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ApplicationSecurables");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ApplicationConfigurations");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Addresses");
        }
    }
}
