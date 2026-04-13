using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spotify.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFotoPlaylis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Fotoplaylist",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fotoplaylist",
                table: "Playlists");
        }
    }
}
