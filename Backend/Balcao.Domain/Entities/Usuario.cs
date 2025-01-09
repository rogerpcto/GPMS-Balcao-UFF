using System.Text.Json.Serialization;

namespace Balcao.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        [JsonIgnore]
        public string Senha { get; set; }
        public float Nota { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual List<Compra> Compras { get; set; } = new List<Compra>();

        public bool Logar(string requisicaoSenha)
        {
            return BCrypt.Net.BCrypt.Verify(requisicaoSenha, Senha);
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public static string Criptografar(string senha)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(10);
            return BCrypt.Net.BCrypt.HashPassword(senha, salt);
        }

        public void Avaliar(float nota)
        {
            int comprasConcluidas = Compras.Where(c => c.Status >= StatusCompra.VENDEDOR_AVALIADO).Count() + Compras.Where(c => c.Status >= StatusCompra.COMPRADOR_AVALIADO).Count();
            Nota = ((Nota * comprasConcluidas) + nota) / (comprasConcluidas + 1);
        }
    }
}
