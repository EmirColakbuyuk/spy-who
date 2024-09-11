using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpyFallBackend.Migrations
{
    /// <inheritdoc />
    public partial class FixCircularDependency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_Players_HostId",
                table: "GameSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_GameSessions_GameSessionId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "WordLists");

            migrationBuilder.RenameColumn(
                name: "RoomCode",
                table: "GameSessions",
                newName: "Word");

            migrationBuilder.RenameColumn(
                name: "HostId",
                table: "GameSessions",
                newName: "SpyId");

            migrationBuilder.RenameIndex(
                name: "IX_GameSessions_HostId",
                table: "GameSessions",
                newName: "IX_GameSessions_SpyId");

            migrationBuilder.AlterColumn<int>(
                name: "GameSessionId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRevealed",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_Players_SpyId",
                table: "GameSessions",
                column: "SpyId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_GameSessions_GameSessionId",
                table: "Players",
                column: "GameSessionId",
                principalTable: "GameSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_Players_SpyId",
                table: "GameSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_GameSessions_GameSessionId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "IsRevealed",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "Word",
                table: "GameSessions",
                newName: "RoomCode");

            migrationBuilder.RenameColumn(
                name: "SpyId",
                table: "GameSessions",
                newName: "HostId");

            migrationBuilder.RenameIndex(
                name: "IX_GameSessions_SpyId",
                table: "GameSessions",
                newName: "IX_GameSessions_HostId");

            migrationBuilder.AlterColumn<int>(
                name: "GameSessionId",
                table: "Players",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GameSessionId = table.Column<int>(type: "int", nullable: false),
                    SpyPlayerId = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rounds_GameSessions_GameSessionId",
                        column: x => x.GameSessionId,
                        principalTable: "GameSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Words = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordLists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_GameSessionId",
                table: "Rounds",
                column: "GameSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_Players_HostId",
                table: "GameSessions",
                column: "HostId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_GameSessions_GameSessionId",
                table: "Players",
                column: "GameSessionId",
                principalTable: "GameSessions",
                principalColumn: "Id");
        }
    }
}
