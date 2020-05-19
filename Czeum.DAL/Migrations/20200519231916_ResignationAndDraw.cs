using Microsoft.EntityFrameworkCore.Migrations;

namespace Czeum.DAL.Migrations
{
    public partial class ResignationAndDraw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Resigned",
                table: "UserMatches",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VotesForDraw",
                table: "UserMatches",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resigned",
                table: "UserMatches");

            migrationBuilder.DropColumn(
                name: "VotesForDraw",
                table: "UserMatches");
        }
    }
}
