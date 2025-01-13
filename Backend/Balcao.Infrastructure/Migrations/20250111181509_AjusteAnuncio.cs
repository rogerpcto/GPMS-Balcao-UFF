using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Balcao.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjusteAnuncio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Anuncios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Contato",
                table: "Anuncios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Localizacao",
                table: "Anuncios",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoAnuncio",
                table: "Anuncios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Anuncios");

            migrationBuilder.DropColumn(
                name: "Contato",
                table: "Anuncios");

            migrationBuilder.DropColumn(
                name: "Localizacao",
                table: "Anuncios");

            migrationBuilder.DropColumn(
                name: "TipoAnuncio",
                table: "Anuncios");
        }
    }
}
