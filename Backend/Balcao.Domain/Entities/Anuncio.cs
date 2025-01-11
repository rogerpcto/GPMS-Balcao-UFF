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
        public string Localizacao { get; set; }
        public string Contato { get; set; }
        public Categoria Categoria { get; set; }
        public TipoAnuncio TipoAnuncio { get; set; }
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

        public Compra IniciarCompra(Usuario autor, int quantidade)
        {
            var compra = new Compra();

            compra.Autor = autor;
            compra.Quantidade = quantidade;

            if (TipoAnuncio == TipoAnuncio.BUSCA)
                compra.Status = StatusCompra.AGUARDANDO_PAGAMENTO;

            Compras.Add(compra);

            return compra;
        }

        public void Avaliar(float nota)
        {
            int comprasConcluidas = Compras.Where(c => c.Status >= StatusCompra.ANUNCIO_AVALIADO).Count();
            Nota = ((Nota * comprasConcluidas) + nota) / (comprasConcluidas + 1);
            Proprietario.Avaliar(nota);
        }

        public void AtualizarQuantidade(int quantidade)
        {
            if (!EhServico())
            {
                Quantidade -= quantidade;
                if (Quantidade <= 0)
                {
                    Desativar();
                }
            }
        }

        public void Desativar()
        {
            Ativo = false;
            Compras.ForEach(c =>
            {
                if (c.Status < StatusCompra.PRODUTO_RECEBIDO)
                    c.Status = StatusCompra.CANCELADO;
            });
        }

        public object ToJson()
        {
            return new
            {
                Id,
                Titulo,
                Descricao,
                Ativo,
                Nota,
                DataCriacao,
                Preco,
                Quantidade,
                Localizacao,
                Contato,
                Categoria,
                TipoAnuncio,
                Proprietario = Proprietario.ToJson(),
                Imagem,
            };
        }
    }
}
