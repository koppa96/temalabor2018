using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Czeum.DAL.Migrations
{
    public partial class MatchCreateAndLastMoveDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Matches",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMove",
                table: "Matches",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "LastMove",
                table: "Matches");
        }
    }
}
