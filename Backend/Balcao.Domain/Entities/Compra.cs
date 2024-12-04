namespace Balcao.Domain.Entities
{
    public class Compra
    {
        public int Id { get; set; }
        public StatusCompra Status { get; set; }
        public Usuario Comprador { get; set; }
        public Usuario Autor { get; set; }
        public Anuncio Assunto { get; set; }
        public List<Mensagem> Mensagens { get; set; } = new List<Mensagem>();

        public void AguardarPagamento()
        {
            throw new NotImplementedException();
        }

        public void EfetuarPagamento()
        {
            throw new NotImplementedException();
        }

        public void ConfirmarPagamento()
        {
            throw new NotImplementedException();
        }

        public void ConfirmarRecebimento()
        {
            throw new NotImplementedException();
        }

        public void AvaliarComprador()
        {
            throw new NotImplementedException();
        }

        public void AvaliarVendedor()
        {
            throw new NotImplementedException();
        }

        public void FecharCompra()
        {
            throw new NotImplementedException();
        }

        public void CancelarCompra()
        {
            throw new NotImplementedException();
        }
    }
}
