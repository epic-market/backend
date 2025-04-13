using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedTimingAndSpectialNoteAndSocialMediaLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SocialMediaLinkFacebook",
                table: "Outlets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialMediaLinkInstagram",
                table: "Outlets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialMediaLinkTwitter",
                table: "Outlets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialMediaLinkYoutube",
                table: "Outlets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialNoteOfTheDay",
                table: "Outlets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimingList",
                table: "Outlets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SocialMediaLinkFacebook",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "SocialMediaLinkInstagram",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "SocialMediaLinkTwitter",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "SocialMediaLinkYoutube",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "SpecialNoteOfTheDay",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "TimingList",
                table: "Outlets");
        }
    }
}
