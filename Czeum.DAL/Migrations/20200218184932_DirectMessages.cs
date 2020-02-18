using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Czeum.DAL.Migrations
{
    public partial class DirectMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Matches_MatchId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "MatchMessages");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_SenderId",
                table: "MatchMessages",
                newName: "IX_MatchMessages_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_MatchId",
                table: "MatchMessages",
                newName: "IX_MatchMessages_MatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchMessages",
                table: "MatchMessages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DirectMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    FriendshipId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DirectMessages_Friendships_FriendshipId",
                        column: x => x.FriendshipId,
                        principalTable: "Friendships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DirectMessages_FriendshipId",
                table: "DirectMessages",
                column: "FriendshipId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchMessages_Matches_MatchId",
                table: "MatchMessages",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchMessages_AspNetUsers_SenderId",
                table: "MatchMessages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchMessages_Matches_MatchId",
                table: "MatchMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchMessages_AspNetUsers_SenderId",
                table: "MatchMessages");

            migrationBuilder.DropTable(
                name: "DirectMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchMessages",
                table: "MatchMessages");

            migrationBuilder.RenameTable(
                name: "MatchMessages",
                newName: "Messages");

            migrationBuilder.RenameIndex(
                name: "IX_MatchMessages_SenderId",
                table: "Messages",
                newName: "IX_Messages_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchMessages_MatchId",
                table: "Messages",
                newName: "IX_Messages_MatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Matches_MatchId",
                table: "Messages",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
