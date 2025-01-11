using Balcao.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Balcao.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoPopularBanco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var senha = Usuario.Criptografar("123");

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Nome", "Email", "Senha", "Nota", "Perfil" },
                values: new object[,]
                {
                    { 1, "Admin", "admin@email.com", senha, 0, "ADMINISTRADOR" },
                    { 2, "João Silva", "joao@email.com", senha, 4.53f, "USUARIO" },
                    { 3, "Maria Oliveira", "maria@email.com", senha, 4.75f, "USUARIO" },
                    { 4, "Igor Santos", "igor@email.com", senha, 4.6f, "USUARIO" },
                    { 5, "Patrick Martins", "patrick@email.com", senha, 4.6f, "USUARIO" },
                });

            migrationBuilder.InsertData(
                table: "Anuncios",
                columns: new[] { "Id", "Titulo", "Descricao", "Preco", "Nota", "Quantidade", "DataCriacao", "Ativo", "ProprietarioId" },
                values: new object[,]
                {
                    { 1, "Cadeira Gamer", "Cadeira confortável para gamers", 499.99f, 4.5f, 10, DateTime.UtcNow, true, 2 },
                    { 2, "Notebook Dell", "Notebook usado, mas em ótimo estado", 1599.00f, 4.3f, 3, DateTime.UtcNow, true, 2 },
                    { 3, "Fone de Ouvido Bluetooth", "Som de alta qualidade e bateria duradoura", 199.90f, 4.8f, 15, DateTime.UtcNow, true, 2 },
                    { 4, "Mesa para Escritório", "Mesa de madeira com gavetas", 349.90f, 4.1f, 5, DateTime.UtcNow, true, 4 },
                    { 5, "Celular Samsung Galaxy", "Celular novo com 128GB de armazenamento", 2199.99f, 4.7f, 8, DateTime.UtcNow, true, 4 },
                    { 6, "Aula de Violão", "Aulas de violão para iniciantes e intermediários", 99.99f, 4.9f, -1, DateTime.UtcNow, true, 5 },
                    { 7, "Consultoria de Marketing Digital", "Consultoria para estratégias de marketing online", 499.00f, 5.0f, -1, DateTime.UtcNow, true, 3 },
                    { 8, "Aula de Programação", "Introdução à programação com Python", 149.90f, 4.6f, -1, DateTime.UtcNow, true, 4 },
                    { 9, "Personal Trainer Online", "Treinamento personalizado e acompanhamento remoto", 250.00f, 4.8f, -1, DateTime.UtcNow, true, 5 },
                    { 10, "Aula de Inglês", "Conversação e gramática para todos os níveis", 120.00f, 4.5f, -1, DateTime.UtcNow, true, 3 }
                });


            migrationBuilder.InsertData(
                table: "Compras",
                columns: new[] { "Id", "AssuntoId", "CompradorId", "Nota", "Quantidade", "Status" },
                values: new object[,]
                {
                    { 1, 1, 4, 0.0f, 1, "NEGOCIANDO" },
                    { 2, 2, 3, 0.0f, 2, "PAGAMENTO_CONFIRMADO" },
                    { 3, 1, 4, 0.0f, 1, "AGUARDANDO_PAGAMENTO" },
                    { 4, 3, 5, 4.5f, 2, "VENDEDOR_AVALIADO" },
                    { 5, 6, 2, 5.0f, 1, "CONCLUIDO" },
                    { 6, 7, 5, 4.9f, 1, "COMPRADOR_AVALIADO" },
                    { 7, 8, 3, 0.0f, 1, "PAGAMENTO_EFETUADO" },
                    { 8, 9, 4, 4.8f, 1, "CONCLUIDO" },
                    { 9, 10, 5, 0.0f, 1, "NEGOCIANDO" },
                    { 10, 5, 3, 0.0f, 1, "PRODUTO_RECEBIDO" }
                });

            migrationBuilder.InsertData(
            table: "Mensagem",
            columns: new[] { "Id", "TimeStamp", "Conteudo", "CompraId" },
            values: new object[,]
            {
                { 1, DateTime.UtcNow.AddDays(-5), "Olá! Gostaria de mais informações sobre a cadeira.", 1 },
                { 2, DateTime.UtcNow.AddDays(-4).AddHours(2), "A cadeira ainda está disponível para compra?", 1 },
                { 3, DateTime.UtcNow.AddDays(-4).AddHours(3), "Sim, está disponível. Posso reservar para você.", 1 },
                { 4, DateTime.UtcNow.AddDays(-3).AddHours(1), "Gostaria de saber se a entrega é gratuita.", 1 },
                { 5, DateTime.UtcNow.AddDays(-2).AddMinutes(45), "Pagamento confirmado. Produto será enviado em breve.", 1 },
                { 6, DateTime.UtcNow.AddDays(-1).AddMinutes(30), "Recebi o produto. A entrega foi muito rápida!", 1 },

                { 7, DateTime.UtcNow.AddDays(-6), "Qual o estado de conservação do notebook?", 2 },
                { 8, DateTime.UtcNow.AddDays(-5).AddHours(4), "O notebook está em excelente estado, com poucos sinais de uso.", 2 },
                { 9, DateTime.UtcNow.AddDays(-4).AddMinutes(15), "Há garantia disponível?", 2 },
                { 10, DateTime.UtcNow.AddDays(-2).AddHours(5), "Sim, oferecemos uma garantia de 3 meses.", 2 },
                { 11, DateTime.UtcNow.AddHours(-6), "Recebi o produto e estou muito satisfeito com a compra.", 2 },
                { 12, DateTime.UtcNow.AddHours(-2), "Obrigado pelo feedback positivo! Agradecemos a preferência.", 2 }
            });

            migrationBuilder.InsertData(
                table: "Imagem",
                columns: new[] { "Id", "Url", "AnuncioId" },
                values: new object[,]
                {
                    { 1, "1_1.jpg", 1 },
                    { 2, "1_2.jpg", 1 },
                    { 3, "1_3.jpg", 1 },
                    { 4, "2_1.jpg", 2 },
                    { 5, "2_2.jpg", 2 },
                    { 6, "2_3.jpg", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
