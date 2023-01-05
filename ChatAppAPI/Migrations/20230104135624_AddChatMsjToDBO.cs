using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddChatMsjToDBO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMsjModels",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonID = table.Column<int>(type: "int", nullable: false),
                    ReferenceID = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMsjModels", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChatMsjModels_ChatModels_ReferenceID",
                        column: x => x.ReferenceID,
                        principalTable: "ChatModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMsjModels_PersonModels_PersonID",
                        column: x => x.PersonID,
                        principalTable: "PersonModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMsjModels_PersonID",
                table: "ChatMsjModels",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMsjModels_ReferenceID",
                table: "ChatMsjModels",
                column: "ReferenceID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMsjModels");
        }
    }
}
