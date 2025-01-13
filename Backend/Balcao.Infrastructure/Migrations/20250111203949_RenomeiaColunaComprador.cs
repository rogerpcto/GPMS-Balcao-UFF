using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Balcao.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenomeiaColunaComprador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Usuarios_CompradorId",
                table: "Compras");

            migrationBuilder.RenameColumn(
                name: "CompradorId",
                table: "Compras",
                newName: "AutorId");

            migrationBuilder.RenameIndex(
                name: "IX_Compras_CompradorId",
                table: "Compras",
                newName: "IX_Compras_AutorId");

            migrationBuilder.AddColumn<int>(
                name: "ComprasConcluidas",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Usuarios_AutorId",
                table: "Compras",
                column: "AutorId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Usuarios_AutorId",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "ComprasConcluidas",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "AutorId",
                table: "Compras",
                newName: "CompradorId");

            migrationBuilder.RenameIndex(
                name: "IX_Compras_AutorId",
                table: "Compras",
                newName: "IX_Compras_CompradorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Usuarios_CompradorId",
                table: "Compras",
                column: "CompradorId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
