using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class BookCatalogAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookList_BookListId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "BookList");

            migrationBuilder.RenameColumn(
                name: "BookListId",
                table: "Books",
                newName: "BookCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_BookListId",
                table: "Books",
                newName: "IX_Books_BookCatalogId");

            migrationBuilder.CreateTable(
                name: "BookCatalog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IsPublic = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCatalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookCatalog_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCatalog_UserId",
                table: "BookCatalog",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookCatalog_BookCatalogId",
                table: "Books",
                column: "BookCatalogId",
                principalTable: "BookCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookCatalog_BookCatalogId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "BookCatalog");

            migrationBuilder.RenameColumn(
                name: "BookCatalogId",
                table: "Books",
                newName: "BookListId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_BookCatalogId",
                table: "Books",
                newName: "IX_Books_BookListId");

            migrationBuilder.CreateTable(
                name: "BookList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    IsPublic = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookList_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookList_UserId",
                table: "BookList",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookList_BookListId",
                table: "Books",
                column: "BookListId",
                principalTable: "BookList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
