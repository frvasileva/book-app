using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class BookListAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookListId",
                table: "Books",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IsPublic = table.Column<bool>(nullable: false),
                    UserId = table.Column<bool>(nullable: false),
                    UserId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookList_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookListId",
                table: "Books",
                column: "BookListId");

            migrationBuilder.CreateIndex(
                name: "IX_BookList_UserId1",
                table: "BookList",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookList_BookListId",
                table: "Books",
                column: "BookListId",
                principalTable: "BookList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookList_BookListId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "BookList");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookListId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookListId",
                table: "Books");
        }
    }
}
