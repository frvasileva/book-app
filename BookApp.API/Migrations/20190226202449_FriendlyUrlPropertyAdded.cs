using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class FriendlyUrlPropertyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FriendlyUrl",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FriendlyUrl",
                table: "Publishers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FriendlyUrl",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendlyUrl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FriendlyUrl",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "FriendlyUrl",
                table: "Books");
        }
    }
}
