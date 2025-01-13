namespace Balcao.Domain.Entities
{
    public enum StatusCompra
    {
        NEGOCIANDO,
        AGUARDANDO_PAGAMENTO,
        PAGAMENTO_EFETUADO,
        PAGAMENTO_CONFIRMADO,
        PRODUTO_RECEBIDO,
        ANUNCIO_AVALIADO,
        COMPRA_AVALIADA,
        CONCLUIDO,
        CANCELADO
    }
}
