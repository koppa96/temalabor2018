using Microsoft.EntityFrameworkCore.Migrations;

namespace Connect4Server.Data.Migrations
{
    public partial class UpdatingNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Result",
                table: "Matches",
                newName: "BoardData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BoardData",
                table: "Matches",
                newName: "Result");
        }
    }
}
