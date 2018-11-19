using Microsoft.EntityFrameworkCore.Migrations;

namespace Connect4Server.Data.Migrations
{
    public partial class GameStateToEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "State",
                table: "Matches",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Matches",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
