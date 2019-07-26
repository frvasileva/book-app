using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class BookCatalogPreferencesAdded2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IconPath",
                table: "BookCatalogPreferences",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "FkTag",
                table: "BookCatalogPreferences",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FkTag",
                table: "BookCatalogPreferences");

            migrationBuilder.AlterColumn<int>(
                name: "IconPath",
                table: "BookCatalogPreferences",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
