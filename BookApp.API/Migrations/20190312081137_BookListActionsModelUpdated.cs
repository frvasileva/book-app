using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class BookListActionsModelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookList_Users_UserId1",
                table: "BookList");

            migrationBuilder.DropIndex(
                name: "IX_BookList_UserId1",
                table: "BookList");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "BookList");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "BookList",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "BookList",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_BookList_UserId",
                table: "BookList",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookList_Users_UserId",
                table: "BookList",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookList_Users_UserId",
                table: "BookList");

            migrationBuilder.DropIndex(
                name: "IX_BookList_UserId",
                table: "BookList");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "BookList");

            migrationBuilder.AlterColumn<bool>(
                name: "UserId",
                table: "BookList",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "BookList",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookList_UserId1",
                table: "BookList",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BookList_Users_UserId1",
                table: "BookList",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
