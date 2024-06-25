using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReactionType = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReactionsToFiles",
                columns: table => new
                {
                    FileId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReactionId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionsToFiles", x => new { x.UserId, x.FileId });
                    table.ForeignKey(
                        name: "FK_ReactionsToFiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReactionsToFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReactionsToFiles_Reactions_ReactionId",
                        column: x => x.ReactionId,
                        principalTable: "Reactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Reactions",
                columns: new[] { "Id", "Path", "ReactionType" },
                values: new object[,]
                {
                    { 1, "~/images/like.png", 0 },
                    { 2, "~/images/dislike.png", 1 },
                    { 3, "~/images/angry.png", 3 },
                    { 4, "~/images/heart.png", 2 },
                    { 5, "~/images/cry.png", 5 },
                    { 6, "~/images/laugh.png", 4 },
                    { 7, "~/images/surprised.png", 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReactionsToFiles_FileId",
                table: "ReactionsToFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionsToFiles_ReactionId",
                table: "ReactionsToFiles",
                column: "ReactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReactionsToFiles");

            migrationBuilder.DropTable(
                name: "Reactions");
        }
    }
}
