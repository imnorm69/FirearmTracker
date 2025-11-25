using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirearmTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarContentType",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "AvatarImage",
                table: "Users",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarContentType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvatarImage",
                table: "Users");
        }
    }
}
