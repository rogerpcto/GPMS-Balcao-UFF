namespace Balcao.Domain.Entities
{
    public class Compra
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public float Nota { get; set; } = -1;
        public StatusCompra Status { get; set; } = StatusCompra.NEGOCIANDO;
        public virtual Usuario Autor { get; set; }
        public virtual Anuncio Assunto { get; set; }
        public virtual List<Mensagem> Mensagens { get; set; } = new List<Mensagem>();

        public Usuario Comprador() => Assunto.TipoAnuncio == TipoAnuncio.OFERTA ? Autor : Assunto.Proprietario;
        public Usuario Vendedor() => Assunto.TipoAnuncio != TipoAnuncio.OFERTA ? Autor : Assunto.Proprietario;

        public void AguardarPagamento()
        {
            Status = StatusCompra.AGUARDANDO_PAGAMENTO;
        }

        public void EfetuarPagamento()
        {
            Status = StatusCompra.PAGAMENTO_EFETUADO;
        }

        public void ConfirmarPagamento()
        {
            Status = StatusCompra.PAGAMENTO_CONFIRMADO;
        }

        public void ConfirmarRecebimento()
        {
            Status = StatusCompra.PRODUTO_RECEBIDO;
            Assunto.AtualizarQuantidade(Quantidade);
        }

        public void AvaliarAnuncio(float nota)
        {
            Nota = nota;
            Assunto.Avaliar(nota);
            Status = StatusCompra.ANUNCIO_AVALIADO;
        }

        public void Avaliar(float nota)
        {
            Autor.Avaliar(nota);
            Status = StatusCompra.COMPRA_AVALIADA;
        }

        public void FecharCompra()
        {
            Status = StatusCompra.CONCLUIDO;
        }

        public void CancelarCompra()
        {
            Status = StatusCompra.CANCELADO;
        }

        public object ToJson()
        {
            return new
            {
                Id,
                Quantidade,
                Nota,
                Status,
                Autor = Autor.ToJson(),
                Assunto = Assunto.ToJson(),
                Mensagens
            };
        }
    }
}
