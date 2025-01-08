namespace Balcao.Domain.Entities
{
    public enum StatusCompra
    {
        NEGOCIANDO,
        AGUARDANDO_PAGAMENTO,
        PAGAMENTO_EFETUADO,
        PAGAMENTO_CONFIRMADO,
        PRODUTO_RECEBIDO,
        VENDEDOR_AVALIADO,
        COMPRADOR_AVALIADO,
        CONCLUIDO,
        CANCELADO
    }
}
