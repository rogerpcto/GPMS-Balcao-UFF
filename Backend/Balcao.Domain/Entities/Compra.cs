namespace Balcao.Domain.Entities
{
    public class Compra
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public float Nota { get; set; } = -1;
        public StatusCompra Status { get; set; } = StatusCompra.NEGOCIANDO;
        public virtual Usuario Comprador { get; set; }
        public virtual Usuario Autor { get; set; }
        public virtual Anuncio Assunto { get; set; }
        public virtual List<Mensagem> Mensagens { get; set; } = new List<Mensagem>();

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

        public void AvaliarVendedor(float nota)
        {
            Nota = nota;
            Assunto.Avaliar(nota);
            Autor.Avaliar(nota);
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
