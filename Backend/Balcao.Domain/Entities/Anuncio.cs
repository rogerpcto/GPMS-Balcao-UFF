namespace Balcao.Domain.Entities
{
    public class Anuncio
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public float Nota { get; set; }
        public DateTime DataCriacao { get; set; }
        public float Preco { get; set; }
        public int Quantidade { get; set; }
        public Usuario Proprietario { get; set; }
        public List<Imagem> Imagem { get; set; } = new List<Imagem>();
        public List<Compra> Compras { get; set; } = new List<Compra>();

        public bool EhServico()
        {
            throw new NotImplementedException();
        }

        public void Pausar()
        {
            throw new NotImplementedException();
        }

        public Compra IniciarCompra(Usuario comprador)
        {
            throw new NotImplementedException();
        }
    }
}
