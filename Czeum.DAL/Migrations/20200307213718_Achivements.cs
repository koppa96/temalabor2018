using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Czeum.DAL.Migrations
{
    public partial class Achivements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsQuickMatch",
                table: "Matches",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MoveCount",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Achivements",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Level = table.Column<int>(nullable: true),
                    MoveCount = table.Column<int>(nullable: true),
                    HaveWinRateAchivement_Level = table.Column<int>(nullable: true),
                    WinRate = table.Column<double>(nullable: true),
                    WinChessMatchesAchivement_Level = table.Column<int>(nullable: true),
                    WinCount = table.Column<int>(nullable: true),
                    WinConnect4MatchesAchivement_Level = table.Column<int>(nullable: true),
                    WinConnect4MatchesAchivement_WinCount = table.Column<int>(nullable: true),
                    WinMatchesAchivement_Level = table.Column<int>(nullable: true),
                    WinMatchesAchivement_WinCount = table.Column<int>(nullable: true),
                    WinQuickMatchesAchivement_Level = table.Column<int>(nullable: true),
                    WinQuickMatchesAchivement_WinCount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achivements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAchivements",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    AchivementId = table.Column<Guid>(nullable: false),
                    UnlockedAt = table.Column<DateTime>(nullable: false),
                    IsStarred = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAchivements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAchivements_Achivements_AchivementId",
                        column: x => x.AchivementId,
                        principalTable: "Achivements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAchivements_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Achivements",
                columns: new[] { "Id", "Discriminator", "Level", "MoveCount" },
                values: new object[] { new Guid("11b89499-17be-4c93-b561-b6cf176f1a52"), "DoMovesAchivement", 1, 1 });

            migrationBuilder.InsertData(
                table: "Achivements",
                columns: new[] { "Id", "Discriminator", "WinQuickMatchesAchivement_Level", "WinQuickMatchesAchivement_WinCount" },
                values: new object[] { new Guid("afa121e1-6dd5-49ea-88d2-5cfd538256f5"), "WinQuickMatchesAchivement", 1, 5 });

            migrationBuilder.InsertData(
                table: "Achivements",
                columns: new[] { "Id", "Discriminator", "WinMatchesAchivement_Level", "WinMatchesAchivement_WinCount" },
                values: new object[,]
                {
                    { new Guid("5bcac04d-2bc4-40ad-af06-4e8cff05945f"), "WinMatchesAchivement", 3, 1000 },
                    { new Guid("d742e2b6-8dd3-4fb8-b80a-728d686251b4"), "WinMatchesAchivement", 2, 100 },
                    { new Guid("69d3d5b7-de45-4bda-b20f-0f4cd5ac3340"), "WinMatchesAchivement", 1, 10 }
                });

            migrationBuilder.InsertData(
                table: "Achivements",
                columns: new[] { "Id", "Discriminator", "WinConnect4MatchesAchivement_Level", "WinConnect4MatchesAchivement_WinCount" },
                values: new object[,]
                {
                    { new Guid("24d59ba8-ff4e-442c-b76f-fbf524e5514d"), "WinConnect4MatchesAchivement", 3, 100 },
                    { new Guid("22c4ee6b-7451-4c81-8010-0ab28571daf7"), "WinConnect4MatchesAchivement", 2, 25 },
                    { new Guid("9982fdd1-8026-4e55-a448-62218cfa4b0a"), "WinConnect4MatchesAchivement", 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Achivements",
                columns: new[] { "Id", "Discriminator", "WinChessMatchesAchivement_Level", "WinCount" },
                values: new object[,]
                {
                    { new Guid("e18f8521-0978-4cf4-b123-e88bccaf992e"), "WinChessMatchesAchivement", 3, 100 },
                    { new Guid("dc3fd2c0-c173-4012-b51d-e64fdd7680eb"), "WinChessMatchesAchivement", 2, 25 },
                    { new Guid("205a0c88-45e4-4bf4-bb0a-be66c18dcd86"), "WinChessMatchesAchivement", 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Achivements",
                columns: new[] { "Id", "Discriminator", "HaveWinRateAchivement_Level", "WinRate" },
                values: new object[,]
                {
                    { new Guid("d62bf078-d3e3-40c5-92cb-a6dda1246327"), "HaveWinRateAchivement", 3, 0.80000000000000004 },
                    { new Guid("855904dd-16ae-41ee-a5c2-cdfc6f4a914f"), "HaveWinRateAchivement", 2, 0.69999999999999996 },
                    { new Guid("68e19be4-e065-4231-9806-bf40a9d0b004"), "HaveWinRateAchivement", 1, 0.59999999999999998 }
                });

            migrationBuilder.InsertData(
                table: "Achivements",
                columns: new[] { "Id", "Discriminator", "Level", "MoveCount" },
                values: new object[,]
                {
                    { new Guid("5f86c4fe-8d0b-40bf-be7f-e5f374184d9c"), "DoMovesAchivement", 3, 5000 },
                    { new Guid("b23e061b-fd81-4335-a843-e34bc78679b1"), "DoMovesAchivement", 2, 500 }
                });

            migrationBuilder.InsertData(
                table: "Achivements",
                columns: new[] { "Id", "Discriminator", "WinQuickMatchesAchivement_Level", "WinQuickMatchesAchivement_WinCount" },
                values: new object[,]
                {
                    { new Guid("7ce1cbff-3fca-4728-9be1-6f632a5e9358"), "WinQuickMatchesAchivement", 2, 50 },
                    { new Guid("12944108-9bb0-43e3-b1c8-f3eac1331442"), "WinQuickMatchesAchivement", 3, 500 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAchivements_AchivementId",
                table: "UserAchivements",
                column: "AchivementId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAchivements_UserId",
                table: "UserAchivements",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAchivements");

            migrationBuilder.DropTable(
                name: "Achivements");

            migrationBuilder.DropColumn(
                name: "IsQuickMatch",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MoveCount",
                table: "AspNetUsers");
        }
    }
}
