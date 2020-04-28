using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Czeum.DAL.Migrations
{
    public partial class FriendRequestCreationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SentAt",
                table: "Requests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentAt",
                table: "Requests");
        }
    }
}
