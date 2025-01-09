using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Balcao.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nota = table.Column<float>(type: "real", nullable: false),
                    Perfil = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Anuncios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Nota = table.Column<float>(type: "real", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Preco = table.Column<float>(type: "real", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    ProprietarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anuncios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anuncios_Usuarios_ProprietarioId",
                        column: x => x.ProprietarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Nota = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompradorId = table.Column<int>(type: "int", nullable: false),
                    AssuntoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compras_Anuncios_AssuntoId",
                        column: x => x.AssuntoId,
                        principalTable: "Anuncios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Compras_Usuarios_CompradorId",
                        column: x => x.CompradorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Imagem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AnuncioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imagem_Anuncios_AnuncioId",
                        column: x => x.AnuncioId,
                        principalTable: "Anuncios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mensagem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Conteudo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CompraId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mensagem_Compras_CompraId",
                        column: x => x.CompraId,
                        principalTable: "Compras",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anuncios_ProprietarioId",
                table: "Anuncios",
                column: "ProprietarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_AssuntoId",
                table: "Compras",
                column: "AssuntoId");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_CompradorId",
                table: "Compras",
                column: "CompradorId");

            migrationBuilder.CreateIndex(
                name: "IX_Imagem_AnuncioId",
                table: "Imagem",
                column: "AnuncioId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensagem_CompraId",
                table: "Mensagem",
                column: "CompraId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Imagem");

            migrationBuilder.DropTable(
                name: "Mensagem");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "Anuncios");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
