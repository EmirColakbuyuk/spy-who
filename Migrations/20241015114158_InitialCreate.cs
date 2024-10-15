using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpyFallBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordLists",
                columns: table => new
                {
                    WordListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordLists", x => x.WordListId);
                });

            migrationBuilder.CreateTable(
                name: "GameTables",
                columns: table => new
                {
                    GameTableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TableKey = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PlayerCount = table.Column<int>(type: "int", nullable: false),
                    CurrentRound = table.Column<int>(type: "int", nullable: false),
                    GameStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SelectedWord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WordListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTables", x => x.GameTableId);
                    table.ForeignKey(
                        name: "FK_GameTables_WordLists_WordListId",
                        column: x => x.WordListId,
                        principalTable: "WordLists",
                        principalColumn: "WordListId");
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    WordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WordListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WordText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.WordId);
                    table.ForeignKey(
                        name: "FK_Words_WordLists_WordListId",
                        column: x => x.WordListId,
                        principalTable: "WordLists",
                        principalColumn: "WordListId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameTableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsSpy = table.Column<bool>(type: "bit", nullable: false),
                    BoxOpened = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Players_GameTables_GameTableId",
                        column: x => x.GameTableId,
                        principalTable: "GameTables",
                        principalColumn: "GameTableId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameTables_WordListId",
                table: "GameTables",
                column: "WordListId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_GameTableId",
                table: "Players",
                column: "GameTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Words_WordListId",
                table: "Words",
                column: "WordListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Words");

            migrationBuilder.DropTable(
                name: "GameTables");

            migrationBuilder.DropTable(
                name: "WordLists");
        }
    }
}
