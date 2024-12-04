namespace Balcao.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public float Nota { get; set; }
        public Perfil Perfil { get; set; }
        public List<Compra> Compras { get; set; } = new List<Compra>();

        public void Logar()
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
