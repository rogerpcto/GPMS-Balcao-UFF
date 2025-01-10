using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Balcao.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjusteMensagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Proprietario",
                table: "Mensagem",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Proprietario",
                table: "Mensagem");
        }
    }
}
