using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spotify.Migrations
{
    /// <inheritdoc />
    public partial class Reproductor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsFavorito",
                table: "Canciones",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsFavorito",
                table: "Canciones");
        }
    }
}
