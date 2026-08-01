using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EclipseWallsBE.Migrations
{
    /// <inheritdoc />
    public partial class mudançadbsetwallpaperpropriedadestypeofdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Downloads",
                table: "Wallpapers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Downloads",
                table: "Wallpapers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
