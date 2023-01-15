using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppAPI.Migrations
{
    /// <inheritdoc />
    public partial class newSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsModels",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsModels", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NewsModels_MediaModels_MediaID",
                        column: x => x.MediaID,
                        principalTable: "MediaModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
