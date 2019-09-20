using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class DiscussionEntitiesAddedd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscussionItem_Discussions_DiscussionId",
                table: "DiscussionItem");

            migrationBuilder.DropColumn(
                name: "FkDiscussion",
                table: "DiscussionItem");

            migrationBuilder.AlterColumn<int>(
                name: "DiscussionId",
                table: "DiscussionItem",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscussionItem_Discussions_DiscussionId",
                table: "DiscussionItem",
                column: "DiscussionId",
                principalTable: "Discussions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscussionItem_Discussions_DiscussionId",
                table: "DiscussionItem");

            migrationBuilder.AlterColumn<int>(
                name: "DiscussionId",
                table: "DiscussionItem",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "FkDiscussion",
                table: "DiscussionItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscussionItem_Discussions_DiscussionId",
                table: "DiscussionItem",
                column: "DiscussionId",
                principalTable: "Discussions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
