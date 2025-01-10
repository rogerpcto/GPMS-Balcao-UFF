namespace Balcao.Domain.Entities
{
    public class Compra
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public float Nota { get; set; } = -1;
        public StatusCompra Status { get; set; } = StatusCompra.NEGOCIANDO;
        public virtual Usuario Comprador { get; set; }
        public virtual Anuncio Assunto { get; set; }
        public virtual List<Mensagem> Mensagens { get; set; } = new List<Mensagem>();

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

        public void AvaliarVendedor(float nota)
        {
            Nota = nota;
            Assunto.Avaliar(nota);
            Status = StatusCompra.VENDEDOR_AVALIADO;
        }

        public void AvaliarComprador(float nota)
        {
            Comprador.Avaliar(nota);
            Status = StatusCompra.COMPRADOR_AVALIADO;
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
                Comprador = Comprador.ToJson(),
                Assunto = Assunto.ToJson(),
                Mensagens
            };
        }
    }
}
