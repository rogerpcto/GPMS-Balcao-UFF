﻿namespace Balcao.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public float Nota { get; set; }
        public int ComprasConcluidas { get; set; }
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
            ComprasConcluidas++;
            Nota = ((Nota * (ComprasConcluidas - 1)) + nota) / ComprasConcluidas;
        }

        public object ToJson()
        {
            return new
            {
                Id,
                Nome,
                Email,
                Nota,
                Perfil
            };
        }
    }
}
