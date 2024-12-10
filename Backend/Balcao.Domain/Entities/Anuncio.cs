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
        public virtual Usuario Proprietario { get; set; }
        public virtual List<Imagem> Imagem { get; set; } = new List<Imagem>();
        public virtual List<Compra> Compras { get; set; } = new List<Compra>();

        public bool EhServico()
        {
            if (Quantidade < 0)
            {
                return true;
            }
            return false;
        }

        public void Pausar()
        {
            Ativo = false;
        }

        public Compra IniciarCompra(Usuario comprador)
        {
            throw new NotImplementedException();
        }
    }
}
