using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpyFallBackend.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyGameSessionRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_Players_SpyId",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_GameSessions_SpyId",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "SpyId",
                table: "GameSessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpyId",
                table: "GameSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_SpyId",
                table: "GameSessions",
                column: "SpyId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_Players_SpyId",
                table: "GameSessions",
                column: "SpyId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
