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
        public Perfil Perfil { get; set; }
        public List<Compra> Compras { get; set; } = new List<Compra>();

        public bool Logar(string requisicaoSenha, string senha)
        {
            if (string.IsNullOrEmpty(senha))
            {
                return false;
            }

            return VerificarSenha(requisicaoSenha, senha);
        }
        private bool VerificarSenha(string requisicaoSenha, string senha)
        {
            return BCrypt.Net.BCrypt.Verify(requisicaoSenha, senha);
        }
        public void Logout()
        {
            throw new NotImplementedException();
        }
    }


}
