using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Addforeignkeywhenmessagehasaparent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Messages_MainMessageId",
                table: "Messages",
                column: "MainMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_MainMessageId",
                table: "Messages",
                column: "MainMessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_MainMessageId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MainMessageId",
                table: "Messages");
        }
    }
}
