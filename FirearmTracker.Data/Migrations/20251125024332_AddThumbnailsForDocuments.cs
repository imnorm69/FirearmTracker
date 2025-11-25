using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirearmTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddThumbnailsForDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailFileName",
                table: "Documents",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailFileName",
                table: "Documents");
        }
    }
}
