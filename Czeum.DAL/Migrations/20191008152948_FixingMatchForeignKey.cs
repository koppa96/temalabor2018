using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Czeum.DAL.Migrations
{
    public partial class FixingMatchForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_WinnerId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMatch_Matches_MatchId",
                table: "UserMatch");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMatch_AspNetUsers_UserId",
                table: "UserMatch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMatch",
                table: "UserMatch");

            migrationBuilder.RenameTable(
                name: "UserMatch",
                newName: "UserMatches");

            migrationBuilder.RenameIndex(
                name: "IX_UserMatch_UserId",
                table: "UserMatches",
                newName: "IX_UserMatches_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMatch_MatchId",
                table: "UserMatches",
                newName: "IX_UserMatches_MatchId");

            migrationBuilder.AlterColumn<Guid>(
                name: "WinnerId",
                table: "Matches",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMatches",
                table: "UserMatches",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_WinnerId",
                table: "Matches",
                column: "WinnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMatches_Matches_MatchId",
                table: "UserMatches",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMatches_AspNetUsers_UserId",
                table: "UserMatches",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_WinnerId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMatches_Matches_MatchId",
                table: "UserMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMatches_AspNetUsers_UserId",
                table: "UserMatches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMatches",
                table: "UserMatches");

            migrationBuilder.RenameTable(
                name: "UserMatches",
                newName: "UserMatch");

            migrationBuilder.RenameIndex(
                name: "IX_UserMatches_UserId",
                table: "UserMatch",
                newName: "IX_UserMatch_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMatches_MatchId",
                table: "UserMatch",
                newName: "IX_UserMatch_MatchId");

            migrationBuilder.AlterColumn<Guid>(
                name: "WinnerId",
                table: "Matches",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMatch",
                table: "UserMatch",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_WinnerId",
                table: "Matches",
                column: "WinnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMatch_Matches_MatchId",
                table: "UserMatch",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMatch_AspNetUsers_UserId",
                table: "UserMatch",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
