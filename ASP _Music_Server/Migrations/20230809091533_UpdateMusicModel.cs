using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP__Music_Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMusicModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Genre",
                schema: "MusicServer",
                table: "Musics",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                schema: "MusicServer",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fa2c02a0-a639-4c4f-8b96-b81946117d54"),
                column: "RegsterDt",
                value: new DateTime(2023, 8, 9, 12, 15, 33, 816, DateTimeKind.Local).AddTicks(7683));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genre",
                schema: "MusicServer",
                table: "Musics");

            migrationBuilder.UpdateData(
                schema: "MusicServer",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fa2c02a0-a639-4c4f-8b96-b81946117d54"),
                column: "RegsterDt",
                value: new DateTime(2023, 7, 28, 0, 55, 2, 846, DateTimeKind.Local).AddTicks(917));
        }
    }
}
