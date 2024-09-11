using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpyFallBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomCodeAndHostToGameSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HostId",
                table: "GameSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RoomCode",
                table: "GameSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_HostId",
                table: "GameSessions",
                column: "HostId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_Players_HostId",
                table: "GameSessions",
                column: "HostId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_Players_HostId",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_GameSessions_HostId",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "HostId",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "RoomCode",
                table: "GameSessions");
        }
    }
}
