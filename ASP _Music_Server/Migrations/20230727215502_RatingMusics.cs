using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP__Music_Server.Migrations
{
    /// <inheritdoc />
    public partial class RatingMusics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RatingMusic",
                schema: "MusicServer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MusicId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingMusic", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                schema: "MusicServer",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fa2c02a0-a639-4c4f-8b96-b81946117d54"),
                column: "RegsterDt",
                value: new DateTime(2023, 7, 28, 0, 55, 2, 846, DateTimeKind.Local).AddTicks(917));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RatingMusic",
                schema: "MusicServer");

            migrationBuilder.UpdateData(
                schema: "MusicServer",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fa2c02a0-a639-4c4f-8b96-b81946117d54"),
                column: "RegsterDt",
                value: new DateTime(2023, 7, 26, 23, 57, 3, 779, DateTimeKind.Local).AddTicks(9077));
        }
    }
}
